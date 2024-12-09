using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface IRoundRepository
    {
        Task<Round?> GetByIdAsync(Guid id);
        Task<IEnumerable<Round>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task<Round> AddAsync(Round round);
        Task UpdateAsync(Round round);
        Task DeleteAsync(Guid id);
    }
}
