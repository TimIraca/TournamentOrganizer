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
        Task<IEnumerable<Tournament>> GetAllAsync();
        Task<Tournament> GetByIdAsync(Guid id);
        Task<Tournament> GetByIdWithParticipantsAsync(Guid id);
        Task<Tournament> GetByIdWithDetailsAsync(Guid id);
        Task<Tournament> CreateAsync(Tournament tournament);
        Task UpdateAsync(Tournament tournament);
        Task DeleteAsync(Guid id);
    }
}
