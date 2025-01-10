using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateToken(User user);
        Task<bool> ValidateCredentials(string username, string password);
        Task<User> RegisterUser(string username, string password);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
