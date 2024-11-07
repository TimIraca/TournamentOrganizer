using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;


namespace TournamentOrganizer.Core.Services.Implementations
{
    public class TournamentService : ITournamentService
    {
        private readonly IMapper _mapper;
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<TournamentCoreDto> GetTournamentByIdAsync(Guid id)
        {
            var tournament = await _tournamentRepository.GetByIdAsync(id);
            return tournament == null ? null : _mapper.Map<TournamentCoreDto>(tournament);
        }

        public async Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync()
        {
            var tournaments = await _tournamentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TournamentCoreDto>>(tournaments);
        }

        public async Task AddTournamentAsync(TournamentCoreDto tournamentDto)
        {
            var tournament = _mapper.Map<Tournament>(tournamentDto);
            await _tournamentRepository.AddAsync(tournament);
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
    }
}
