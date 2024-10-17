using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.Domain.Interfaces
{
    public interface ITournamentRepository
    {
        Task<Tournament> GetTournamentByIdAsync(Guid id);
        Task<IEnumerable<Tournament>> GetAllTournamentsAsync();
        Task CreateTournamentAsync(Tournament tournament);
        Task UpdateTournamentAsync(Tournament tournament);
        Task DeleteTournamentAsync(Guid id);
    }
}
