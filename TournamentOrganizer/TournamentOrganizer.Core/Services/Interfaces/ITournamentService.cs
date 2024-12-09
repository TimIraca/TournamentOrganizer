using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.DTOs.Overview;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<TournamentCoreDto?> GetTournamentByIdAsync(Guid id);
        Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync();
        Task<TournamentCoreDto> AddTournamentAsync(TournamentCoreDto tournament);
        Task UpdateTournamentAsync(TournamentCoreDto tournament);
        Task DeleteTournamentAsync(Guid id);
        Task<TournamentOverviewDto?> GetTournamentOverviewAsync(Guid tournamentId);
        Task StartTournamentAsync(Guid tournamentId);
    }
}
