using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

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
            RoundCoreDto round = await _roundRepository.GetByIdAsync(id);
            return _mapper.Map<RoundCoreDto>(round);
        }

        public async Task<IEnumerable<RoundCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId)
        {
            IEnumerable<RoundCoreDto> rounds = await _roundRepository.GetAllByTournamentIdAsync(
                tournamentId
            );
            return _mapper.Map<IEnumerable<RoundCoreDto>>(rounds);
        }

        public async Task<RoundCoreDto> AddAsync(RoundCoreDto round)
        {
            RoundCoreDto roundEntity = _mapper.Map<RoundCoreDto>(round);
            RoundCoreDto addedRound = await _roundRepository.AddAsync(roundEntity);
            return _mapper.Map<RoundCoreDto>(addedRound);
        }

        public async Task UpdateAsync(RoundCoreDto round)
        {
            RoundCoreDto roundEntity = _mapper.Map<RoundCoreDto>(round);
            await _roundRepository.UpdateAsync(roundEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _roundRepository.DeleteAsync(id);
        }
    }
}
