using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<TournamentCoreDto> GetTournamentByIdAsync(Guid id);
        Task<IEnumerable<TournamentCoreDto>> GetAllTournamentsAsync();
        Task AddTournamentAsync(TournamentCoreDto tournamentDto);
        Task UpdateTournamentAsync(TournamentCoreDto tournamentDto);
        Task DeleteTournamentAsync(Guid id);
    }
}
