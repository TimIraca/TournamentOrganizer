using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(string username, string password);
        Task<bool> UsernameExistsAsync(string username);
    }
}
