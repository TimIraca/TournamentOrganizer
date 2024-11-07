using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        Task<Tournament> GetByIdAsync(Guid id);
        Task<IEnumerable<Tournament>> GetAllAsync();
        Task AddAsync(Tournament tournament);
        Task UpdateAsync(Tournament tournament);
        Task DeleteAsync(Guid id);
    }
}
