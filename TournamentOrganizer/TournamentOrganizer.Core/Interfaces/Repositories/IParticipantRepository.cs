using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Interfaces.Repositories
{
    public interface IParticipantRepository
    {
        Task<ParticipantCoreDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ParticipantCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task<ParticipantCoreDto> AddAsync(ParticipantCoreDto participant);
        Task UpdateAsync(ParticipantCoreDto participant);
        Task DeleteAsync(Guid id);
    }
}
