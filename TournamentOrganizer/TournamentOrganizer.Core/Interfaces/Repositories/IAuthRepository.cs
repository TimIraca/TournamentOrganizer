using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<UserCoreDto> GetUserByIdAsync(int id);
        Task<UserCoreDto> GetUserByUsernameAsync(string username);
        Task<UserCoreDto> CreateUserAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
    }
}
