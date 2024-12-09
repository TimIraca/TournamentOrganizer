using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Implementations;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.Core.Services.Implementations
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

        public async Task<MatchCoreDto> GetByIdAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            if (match == null)
                throw new NotFoundException($"Match with ID {id} not found");

            return _mapper.Map<MatchCoreDto>(match);
        }

        public async Task<IEnumerable<MatchCoreDto>> GetAllByRoundIdAsync(Guid roundId)
        {
            var matches = await _matchRepository.GetAllByRoundIdAsync(roundId);
            return _mapper.Map<IEnumerable<MatchCoreDto>>(matches);
        }

        public async Task<MatchCoreDto> AddAsync(MatchCoreDto match)
        {
            var matchEntity = _mapper.Map<Match>(match);
            var addedMatch = await _matchRepository.AddAsync(matchEntity);
            return _mapper.Map<MatchCoreDto>(addedMatch);
        }

        public async Task UpdateAsync(MatchCoreDto match)
        {
            var existingMatch = await _matchRepository.GetByIdAsync(match.Id);
            if (existingMatch == null)
                throw new NotFoundException($"Match with ID {match.Id} not found");

            var matchEntity = _mapper.Map<Match>(match);
            await _matchRepository.UpdateAsync(matchEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingMatch = await _matchRepository.GetByIdAsync(id);
            if (existingMatch == null)
                throw new NotFoundException($"Match with ID {id} not found");

            await _matchRepository.DeleteAsync(id);
        }

        public async Task DeclareMatchWinnerAsync(Guid tournamentId, Guid matchId, Guid winnerId)
        {
            var rounds = await _roundRepository.GetAllByTournamentIdAsync(tournamentId);
            if (!rounds.Any())
                throw new NotFoundException($"No rounds found for tournament {tournamentId}");

            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new NotFoundException($"Match with ID {matchId} not found");

            if (match.Participant1Id != winnerId && match.Participant2Id != winnerId)
                throw new InvalidOperationException(
                    $"Player {winnerId} is not a participant in match {matchId}"
                );

            var roundDtos = _mapper.Map<IEnumerable<RoundCoreDto>>(rounds).ToList();
            BracketGenerator.UpdateBracket(roundDtos, winnerId, matchId);

            foreach (var round in roundDtos)
            {
                await _roundRepository.UpdateAsync(_mapper.Map<Round>(round));
            }
        }
    }
}
