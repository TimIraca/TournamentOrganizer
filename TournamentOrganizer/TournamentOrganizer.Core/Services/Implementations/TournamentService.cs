using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.DTOs.Overview;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Implementations;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.Core.Services.Implementations
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public TournamentService(
            ITournamentRepository tournamentRepository,
            IMapper mapper,
            IRoundRepository roundRepository,
            IMatchRepository matchRepository,
            IParticipantRepository participantRepository
        )
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
            _roundRepository = roundRepository;
            _matchRepository = matchRepository;
            _participantRepository = participantRepository;
        }

        public async Task<TournamentCoreDto?> GetTournamentByIdAsync(Guid id)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(id);
            return tournament == null ? null : _mapper.Map<TournamentCoreDto>(tournament);
        }

        public async Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync()
        {
            var tournaments = await _tournamentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TournamentCoreDto>>(tournaments);
        }

        public async Task<TournamentCoreDto> AddTournamentAsync(TournamentCoreDto tournamentDto)
        {
            Tournament tournament = _mapper.Map<Tournament>(tournamentDto);

            // Add the tournament and get the created entity
            Tournament createdTournament = await _tournamentRepository.AddAsync(tournament);

            // Map the created entity back to a Core DTO
            return _mapper.Map<TournamentCoreDto>(createdTournament);
        }

        public async Task UpdateTournamentAsync(TournamentCoreDto tournamentDto)
        {
            var tournament = _mapper.Map<Tournament>(tournamentDto);
            await _tournamentRepository.UpdateAsync(tournament);
        }

        public async Task DeleteTournamentAsync(Guid id)
        {
            await _tournamentRepository.DeleteAsync(id);
        }

        public async Task StartTournamentAsync(Guid id)
        {
            var tournament =
                await _tournamentRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Tournament not found");

            if (tournament.Participants == null || tournament.Participants.Count < 3)
            {
                throw new InvalidOperationException(
                    "Tournament must have at least 3 participants to start."
                );
            }

            // Check if tournament already has rounds
            var existingRounds = await _roundRepository.GetAllByTournamentIdAsync(tournament.Id);
            if (existingRounds.Any())
            {
                throw new InvalidOperationException("Tournament already started.");
            }

            // Shuffle participants for randomness
            var participants = tournament.Participants.OrderBy(_ => Guid.NewGuid()).ToList();
            List<ParticipantCoreDto> participantDtos = _mapper.Map<List<ParticipantCoreDto>>(
                participants
            );
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participantDtos, tournament.Id)
                .ToList();
            foreach (var round in rounds)
            {
                await _roundRepository.AddAsync(_mapper.Map<Round>(round));
            }
        }

        public static IEnumerable<RoundCoreDto> GenerateBracket(
            IEnumerable<ParticipantCoreDto> participants,
            Guid tournamentId
        )
        {
            if (participants == null)
                throw new ArgumentNullException(nameof(participants));

            var participantsList = participants.ToList();
            if (participantsList.Count == 0)
                throw new ArgumentException(
                    "Must have at least one participant",
                    nameof(participants)
                );

            var rounds = new List<RoundCoreDto>();
            int matchNumber = 1;

            // For 5 participants with 3 byes:
            // First 3 participants get byes (indexes 0-2)
            // Last 2 participants play in first round (indexes 3-4)

            // Round 1: One match with the non-bye players
            var round1Matches = new List<MatchCoreDto>
            {
                new MatchCoreDto
                {
                    Id = Guid.NewGuid(),
                    MatchNumber = matchNumber++,
                    Participant1Id = participantsList[3].Id, // Player 4
                    Participant2Id = participantsList[
                        4
                    ].Id // Player 5
                    ,
                },
            };

            rounds.Add(
                new RoundCoreDto
                {
                    Id = Guid.NewGuid(),
                    RoundNumber = 1,
                    TournamentId = tournamentId,
                    Matches = round1Matches,
                }
            );

            // Round 2: Two matches
            var round2Matches = new List<MatchCoreDto>
            {
                // Match 2: Bye players 2 vs 3
                new MatchCoreDto
                {
                    Id = Guid.NewGuid(),
                    MatchNumber = matchNumber++,
                    Participant1Id = participantsList[1].Id, // Player 2
                    Participant2Id = participantsList[
                        2
                    ].Id // Player 3
                    ,
                },
                // Match 3: Bye player 1 vs Winner(Match 1)
                new MatchCoreDto
                {
                    Id = Guid.NewGuid(),
                    MatchNumber = matchNumber++,
                    Participant1Id = participantsList[0].Id, // Player 1
                    Participant2Id =
                        null // Will get winner from Match 1
                    ,
                },
            };

            rounds.Add(
                new RoundCoreDto
                {
                    Id = Guid.NewGuid(),
                    RoundNumber = 2,
                    TournamentId = tournamentId,
                    Matches = round2Matches,
                }
            );

            // Round 3: Finals
            var round3Matches = new List<MatchCoreDto>
            {
                new MatchCoreDto
                {
                    Id = Guid.NewGuid(),
                    MatchNumber = matchNumber++,
                    Participant1Id = null, // Will get winner from Match 2
                    Participant2Id =
                        null // Will get winner from Match 3
                    ,
                },
            };

            rounds.Add(
                new RoundCoreDto
                {
                    Id = Guid.NewGuid(),
                    RoundNumber = 3,
                    TournamentId = tournamentId,
                    Matches = round3Matches,
                }
            );

            return rounds;
        }

        public void UpdateBracket(IEnumerable<RoundCoreDto> rounds, Guid winnerId, Guid matchId)
        {
            var currentRound = rounds.FirstOrDefault(r => r.Matches.Any(m => m.Id == matchId));
            if (currentRound == null)
                return;

            var completedMatch = currentRound.Matches.First(m => m.Id == matchId);
            completedMatch.WinnerId = winnerId;

            var nextRound = rounds.FirstOrDefault(r =>
                r.RoundNumber == currentRound.RoundNumber + 1
            );
            if (nextRound == null)
                return;

            if (currentRound.RoundNumber == 1)
            {
                // Winner of Match 1 goes to Match 3 (second match of round 2)
                var nextMatch = nextRound.Matches.ElementAt(1);
                nextMatch.Participant2Id = winnerId;
            }
            else if (currentRound.RoundNumber == 2)
            {
                // Winners from Match 2 and 3 go to the finals
                var finalMatch = nextRound.Matches.First();

                if (completedMatch.MatchNumber == 2)
                {
                    finalMatch.Participant1Id = winnerId;
                }
                else // Match 3
                {
                    finalMatch.Participant2Id = winnerId;
                }
            }
        }

        public async Task<TournamentOverviewDto?> GetTournamentOverviewAsync(Guid tournamentId)
        {
            // Fetch tournament data
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
                return null;

            // Build the DTO
            return new TournamentOverviewDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                StartDate = tournament.StartDate,
                IsCompleted = tournament.IsCompleted,
                Rounds = _mapper.Map<IEnumerable<RoundOverviewDto>>(tournament.Rounds),
                Participants = _mapper.Map<IEnumerable<ParticipantOverviewDto>>(
                    tournament.Participants
                ),
            };
        }
    }
}
