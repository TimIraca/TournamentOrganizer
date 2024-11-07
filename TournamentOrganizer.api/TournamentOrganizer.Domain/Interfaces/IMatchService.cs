using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.Domain.Interfaces
{
    public interface IMatchService
    {
        Task<Match> GetMatchAsync(Guid id);
        Task<IEnumerable<Match>> GetTournamentMatchesAsync(Guid tournamentId);
        Task<Match> CreateMatchAsync(Match match);
        Task<Match> UpdateMatchAsync(Guid id, Match match);
        Task<Match> UpdateScoreAsync(Guid id, int? participant1Score, int? participant2Score);
        Task<Match> CompleteMatchAsync(Guid id, Guid winnerId);
        Task DeleteMatchAsync(Guid id);
        Task<IEnumerable<Match>> GenerateSingleEliminationBracketAsync(
            Guid tournamentId,
            IList<TournamentParticipant> participants
        );
        Task<IEnumerable<Match>> GetBracketStructureAsync(Guid tournamentId);
        Task AdvanceWinnerAsync(Guid matchId);
    }
}
