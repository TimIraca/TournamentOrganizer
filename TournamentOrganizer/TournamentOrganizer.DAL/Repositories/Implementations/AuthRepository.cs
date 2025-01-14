using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TournamentContext _context;
        private readonly IMapper _mapper;

        public AuthRepository(TournamentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserCoreDto> GetUserByIdAsync(int id)
        {
            UserCoreDto user = _mapper.Map<UserCoreDto>(await _context.Users.FindAsync(id));
            return user;
        }

        public async Task<UserCoreDto> GetUserByUsernameAsync(string username)
        {
            UserCoreDto user = _mapper.Map<UserCoreDto>(
                await _context.Users.FirstOrDefaultAsync(u => u.Username == username)
            );
            return user;
        }

        public async Task<UserCoreDto> CreateUserAsync(string username, string password)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            UserCoreDto usercore = _mapper.Map<UserCoreDto>(user);
            return usercore;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}
