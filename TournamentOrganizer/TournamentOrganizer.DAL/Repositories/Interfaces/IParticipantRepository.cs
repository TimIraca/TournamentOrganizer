using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        Task<TournamentParticipant> GetByIdAsync(Guid id);
        Task<IEnumerable<TournamentParticipant>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task AddAsync(TournamentParticipant participant);
        Task UpdateAsync(TournamentParticipant participant);
        Task DeleteAsync(Guid id);
        Task<int> CountParticipantsAsync(Guid tournamentId);
        Task<Tournament> GetTournamentAsync(Guid tournamentId);
    }
}
