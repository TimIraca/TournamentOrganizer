using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.DTOs.Overview;

namespace TournamentOrganizer.Core.Interfaces.Services
{
    public interface ITournamentService
    {
        Task<TournamentCoreDto?> GetTournamentByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync(Guid userId);
        Task<TournamentCoreDto> AddTournamentAsync(TournamentCoreDto tournament, Guid userId);
        Task UpdateTournamentAsync(TournamentCoreDto tournament, Guid userId);
        Task DeleteTournamentAsync(Guid id, Guid userId);
        Task<TournamentOverviewDto?> GetTournamentOverviewAsync(Guid tournamentId, Guid userId);
        Task StartTournamentAsync(Guid tournamentId, Guid userId);
    }
}
