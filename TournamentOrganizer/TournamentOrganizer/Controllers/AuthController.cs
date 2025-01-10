using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthApiDto model)
        {
            try
            {
                var user = await _authService.RegisterUser(model.Username, model.Password);

                return Ok(new { user.Id, user.Username });
            }
            catch (Exception ex) when (ex.Message == "Username already exists")
            {
                return Conflict(new { message = "Username already exists" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthApiDto model)
        {
            if (!await _authService.ValidateCredentials(model.Username, model.Password))
            {
                return Unauthorized();
            }

            var user = await _authService.GetUserByUsernameAsync(model.Username);
            var token = await _authService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
