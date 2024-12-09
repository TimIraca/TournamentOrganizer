using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant?> GetByIdAsync(Guid id);
        Task<IEnumerable<Participant>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task<Participant> AddAsync(Participant participant);
        Task UpdateAsync(Participant participant);
        Task DeleteAsync(Guid id);
    }
}
