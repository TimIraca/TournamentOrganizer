using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface ITournamentRepository
    {
        Task<TournamentCoreDto?> GetByIdAsync(Guid id, Guid userId);
        Task<IEnumerable<TournamentCoreDto>> GetAllAsync(Guid userId);
        Task<TournamentCoreDto> AddAsync(TournamentCoreDto tournament);
        Task UpdateAsync(TournamentCoreDto tournament, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
