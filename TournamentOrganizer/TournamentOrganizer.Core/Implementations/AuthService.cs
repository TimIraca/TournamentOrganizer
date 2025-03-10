﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.Core.Interfaces.Services;

namespace TournamentOrganizer.Core.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }

        public Task<string> GenerateToken(UserCoreDto user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("uid", user.Id.ToString()),
                new Claim("username", user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])
                ),
                signingCredentials: credentials
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await _authRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<UserCoreDto> RegisterUser(string username, string password)
        {
            if (await _authRepository.UsernameExistsAsync(username))
            {
                throw new Exception("Username already exists");
            }

            return await _authRepository.CreateUserAsync(username, password);
        }

        public async Task<UserCoreDto> GetUserByUsernameAsync(string username)
        {
            return await _authRepository.GetUserByUsernameAsync(username);
        }
    }
}
