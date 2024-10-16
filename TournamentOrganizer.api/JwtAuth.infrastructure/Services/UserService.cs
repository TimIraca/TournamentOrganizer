using JwtAuth.core.Entities;
using JwtAuth.infrastructure.Data;
using JwtAuth.infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using JwtAuth.core.DTOs;
using JwtAuth.core.Interfaces;


namespace JwtAuth.infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IUser> AuthenticateAsync(string identifier, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.PhoneNumber == identifier);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                return null;
            return user;
        }

        public async Task<IUser> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email || u.PhoneNumber == registerDto.PhoneNumber))
                throw new Exception("User with this email or phone number already exists.");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                PasswordHash = HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return storedHash == hashedPassword;
        }
    }

}
