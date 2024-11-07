using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.Domain.Interfaces
{
    public interface IMatchRepository
    {
        Task<Match> GetByIdAsync(Guid id);
        Task<IEnumerable<Match>> GetByTournamentIdAsync(Guid tournamentId);
        Task<IEnumerable<Match>> GetByRoundAsync(Guid tournamentId, int round);
        Task<Match> CreateAsync(Match match);
        Task<Match> UpdateAsync(Match match);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Match>> GetPendingMatchesAsync();
        Task<bool> ExistsAsync(Guid id);
    }
}
