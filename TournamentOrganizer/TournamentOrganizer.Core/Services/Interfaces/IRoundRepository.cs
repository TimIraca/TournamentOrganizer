using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface IRoundRepository
    {
        Task<RoundCoreDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<RoundCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task<RoundCoreDto> AddAsync(RoundCoreDto round);
        Task UpdateAsync(RoundCoreDto round);
        Task DeleteAsync(Guid id);
    }
}
