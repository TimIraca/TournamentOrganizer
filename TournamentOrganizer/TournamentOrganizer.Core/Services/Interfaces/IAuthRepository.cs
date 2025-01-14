using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserCoreDto> GetUserByIdAsync(int id);
        Task<UserCoreDto> GetUserByUsernameAsync(string username);
        Task<UserCoreDto> CreateUserAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
    }
}
