using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface IRoundService
    {
        Task<RoundCoreDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<RoundCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task<RoundCoreDto> AddAsync(RoundCoreDto round);
        Task UpdateAsync(RoundCoreDto round);
        Task DeleteAsync(Guid id);
    }
}
