using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(Guid id);
        Task<IEnumerable<Match>> GetAllByRoundIdAsync(Guid roundId);
        Task<Match> AddAsync(Match match);
        Task UpdateAsync(Match match);
        Task DeleteAsync(Guid id);
    }
}
