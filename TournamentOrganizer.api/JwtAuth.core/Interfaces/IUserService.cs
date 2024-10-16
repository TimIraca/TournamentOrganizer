using JwtAuth.core.DTOs;
using JwtAuth.core.Entities;

namespace JwtAuth.core.Interfaces
{
    public interface IUserService
    {
        Task<IUser> AuthenticateAsync(string identifier, string password);
        Task<IUser> RegisterAsync(RegisterDto registerDto);
        Task<IUser> GetByIdAsync(string id);
    }
}
