using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(UserCoreDto user);
        Task<bool> ValidateCredentials(string username, string password);
        Task<UserCoreDto> RegisterUser(string username, string password);
        Task<UserCoreDto> GetUserByUsernameAsync(string username);
    }
}
