using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.Core.Interfaces.Services;

namespace TournamentOrganizer.Core.Implementations
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IMapper _mapper;

        public MatchService(
            IMatchRepository matchRepository,
            IMapper mapper,
            IRoundRepository roundRepository
        )
        {
            _matchRepository = matchRepository;
            _mapper = mapper;
            _roundRepository = roundRepository;
        }

        public async Task<MatchCoreDto?> GetByIdAsync(Guid id)
        {
            MatchCoreDto? match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
            {
                throw new NotFoundException($"Match with ID {id} not found");
            }

            return _mapper.Map<MatchCoreDto>(match);
        }

        public async Task<IEnumerable<MatchCoreDto>> GetAllByRoundIdAsync(Guid roundId)
        {
            IEnumerable<MatchCoreDto> matches = await _matchRepository.GetAllByRoundIdAsync(
                roundId
            );
            return _mapper.Map<IEnumerable<MatchCoreDto>>(matches);
        }

        public async Task<MatchCoreDto> AddAsync(MatchCoreDto match)
        {
            MatchCoreDto matchEntity = _mapper.Map<MatchCoreDto>(match);
            MatchCoreDto addedMatch = await _matchRepository.AddAsync(matchEntity);
            return _mapper.Map<MatchCoreDto>(addedMatch);
        }

        public async Task UpdateAsync(MatchCoreDto match)
        {
            MatchCoreDto? existingMatch = await _matchRepository.GetByIdAsync(match.Id);
            if (existingMatch == null)
            {
                throw new NotFoundException($"Match with ID {match.Id} not found");
            }

            MatchCoreDto matchEntity = _mapper.Map<MatchCoreDto>(match);
            await _matchRepository.UpdateAsync(matchEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            MatchCoreDto? existingMatch = await _matchRepository.GetByIdAsync(id);
            if (existingMatch == null)
            {
                throw new NotFoundException($"Match with ID {id} not found");
            }

            await _matchRepository.DeleteAsync(id);
        }

        public async Task DeclareMatchWinnerAsync(Guid tournamentId, Guid matchId, Guid winnerId)
        {
            IEnumerable<RoundCoreDto> rounds = await _roundRepository.GetAllByTournamentIdAsync(
                tournamentId
            );
            if (!rounds.Any())
            {
                throw new NotFoundException($"No rounds found for tournament {tournamentId}");
            }

            MatchCoreDto? match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
            {
                throw new NotFoundException($"Match with ID {matchId} not found");
            }

            if (match.Participant1Id != winnerId && match.Participant2Id != winnerId)
            {
                throw new InvalidOperationException(
                    $"Player {winnerId} is not a participant in match {matchId}"
                );
            }

            List<RoundCoreDto> roundDtos = _mapper.Map<IEnumerable<RoundCoreDto>>(rounds).ToList();
            BracketGenerator.UpdateBracket(roundDtos, winnerId, matchId);

            foreach (RoundCoreDto round in roundDtos)
            {
                await _roundRepository.UpdateAsync(_mapper.Map<RoundCoreDto>(round));
            }
        }
    }
}
