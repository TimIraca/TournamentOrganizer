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
        Task<Tournament?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<Tournament>> GetAllAsync(Guid userId);
        Task<Tournament> AddAsync(Tournament tournament);
        Task UpdateAsync(Tournament tournament, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
