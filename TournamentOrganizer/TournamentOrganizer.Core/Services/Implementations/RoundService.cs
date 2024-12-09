using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.Core.Services.Implementations
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IMapper _mapper;

        public RoundService(IRoundRepository roundRepository, IMapper mapper)
        {
            _roundRepository = roundRepository;
            _mapper = mapper;
        }

        public async Task<RoundCoreDto?> GetByIdAsync(Guid id)
        {
            Round round = await _roundRepository.GetByIdAsync(id);
            return _mapper.Map<RoundCoreDto>(round);
        }

        public async Task<IEnumerable<RoundCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId)
        {
            var rounds = await _roundRepository.GetAllByTournamentIdAsync(tournamentId);
            return _mapper.Map<IEnumerable<RoundCoreDto>>(rounds);
        }

        public async Task<RoundCoreDto> AddAsync(RoundCoreDto round)
        {
            var roundEntity = _mapper.Map<Round>(round);
            var addedRound = await _roundRepository.AddAsync(roundEntity);
            return _mapper.Map<RoundCoreDto>(addedRound);
        }

        public async Task UpdateAsync(RoundCoreDto round)
        {
            var roundEntity = _mapper.Map<Round>(round);
            await _roundRepository.UpdateAsync(roundEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _roundRepository.DeleteAsync(id);
        }
    }
}
