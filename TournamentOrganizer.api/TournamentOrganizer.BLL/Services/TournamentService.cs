using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.BLL.Services
{
    public class TournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<IEnumerable<Tournament>> GetAllTournamentsAsync()
        {
            return await _tournamentRepository.GetAllTournamentsAsync();
        }

        public async Task<Tournament> GetTournamentByIdAsync(Guid id)
        {
            return await _tournamentRepository.GetTournamentByIdAsync(id);
        }

        public async Task CreateTournamentAsync(Tournament tournament)
        {
            await _tournamentRepository.CreateTournamentAsync(tournament);
        }

        public async Task UpdateTournamentAsync(Tournament tournament)
        {
            await _tournamentRepository.UpdateTournamentAsync(tournament);
        }

        public async Task DeleteTournamentAsync(Guid id)
        {
            await _tournamentRepository.DeleteTournamentAsync(id);
        }
    }
}
