using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.DTOs.Overview;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.Core.Interfaces.Services;

namespace TournamentOrganizer.Core.Implementations
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

        public async Task<TournamentCoreDto?> GetTournamentByIdAsync(Guid id, Guid userId)
        {
            TournamentCoreDto? tournament = await _tournamentRepository.GetByIdAsync(id, userId);
            return tournament == null ? null : _mapper.Map<TournamentCoreDto>(tournament);
        }

        public async Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync(Guid userId)
        {
            IEnumerable<TournamentCoreDto> tournaments = await _tournamentRepository.GetAllAsync(
                userId
            );
            return _mapper.Map<IEnumerable<TournamentCoreDto>>(tournaments);
        }

        public async Task<TournamentCoreDto> AddTournamentAsync(
            TournamentCoreDto tournamentDto,
            Guid userId
        )
        {
            TournamentCoreDto tournament = _mapper.Map<TournamentCoreDto>(tournamentDto);
            tournament.UserId = userId;
            // Add the tournament and get the created entity
            TournamentCoreDto createdTournament = await _tournamentRepository.AddAsync(tournament);

            // Map the created entity back to a Core DTO
            return createdTournament;
        }

        public async Task UpdateTournamentAsync(TournamentCoreDto tournamentDto, Guid userId)
        {
            TournamentCoreDto tournament = _mapper.Map<TournamentCoreDto>(tournamentDto);
            await _tournamentRepository.UpdateAsync(tournament, userId);
        }

        public async Task DeleteTournamentAsync(Guid id, Guid userId)
        {
            await _tournamentRepository.DeleteAsync(id, userId);
        }

        public async Task StartTournamentAsync(Guid id, Guid userId)
        {
            TournamentCoreDto tournament =
                await _tournamentRepository.GetByIdAsync(id, userId)
                ?? throw new InvalidOperationException("Tournament not found");

            if (tournament.Participants == null || tournament.Participants.Count() < 3)
            {
                throw new InvalidOperationException(
                    "Tournament must have at least 3 participants to start."
                );
            }

            // Check if tournament already has rounds
            IEnumerable<RoundCoreDto> existingRounds =
                await _roundRepository.GetAllByTournamentIdAsync(tournament.Id);
            if (existingRounds.Any())
            {
                throw new InvalidOperationException("Tournament already started.");
            }

            // Shuffle participants for randomness
            List<ParticipantCoreDto> participants = tournament
                .Participants.OrderBy(_ => Guid.NewGuid())
                .ToList();
            List<ParticipantCoreDto> participantDtos = _mapper.Map<List<ParticipantCoreDto>>(
                participants
            );
            List<RoundCoreDto> rounds = BracketGenerator
                .GenerateBracket(participantDtos, tournament.Id)
                .ToList();
            foreach (RoundCoreDto round in rounds)
            {
                await _roundRepository.AddAsync(round);
            }
        }

        public async Task<TournamentOverviewDto?> GetTournamentOverviewAsync(
            Guid tournamentId,
            Guid userId
        )
        {
            // Fetch tournament data
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId, userId);
            if (tournament == null)
            {
                return null;
            }

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

        public async Task ResetTournamentAsync(Guid id, Guid userId)
        {
            TournamentCoreDto tournament =
                await _tournamentRepository.GetByIdAsync(id, userId)
                ?? throw new InvalidOperationException("Tournament not found");
            // Delete all rounds and matches
            IEnumerable<RoundCoreDto> rounds = await _roundRepository.GetAllByTournamentIdAsync(id);
            foreach (RoundCoreDto round in rounds)
            {
                await _roundRepository.DeleteAsync(round.Id);
            }
            // Update tournament to not completed
            tournament.IsCompleted = false;
            await _tournamentRepository.UpdateAsync(tournament, userId);
        }
    }
}
