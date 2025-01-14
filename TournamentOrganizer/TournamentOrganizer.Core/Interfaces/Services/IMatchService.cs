using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Interfaces.Services
{
    public interface IMatchService
    {
        Task<MatchCoreDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<MatchCoreDto>> GetAllByRoundIdAsync(Guid roundId);
        Task<MatchCoreDto> AddAsync(MatchCoreDto match);
        Task UpdateAsync(MatchCoreDto match);
        Task DeleteAsync(Guid id);
        Task DeclareMatchWinnerAsync(Guid tournamentId, Guid matchId, Guid participantId);
    }
}
