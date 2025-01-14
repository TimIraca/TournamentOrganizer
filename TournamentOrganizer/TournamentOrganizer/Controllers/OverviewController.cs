using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.Core.Interfaces.Services;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OverviewController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public OverviewController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                throw new UnauthorizedAccessException("User not properly authenticated");
            }
            return userId;
        }

        // GET: api/overview/tournament/{tournamentId}
        [HttpGet("tournament/{tournamentId}")]
        public async Task<IActionResult> GetTournamentOverview(Guid tournamentId)
        {
            Guid userId = GetCurrentUserId();
            Core.DTOs.Overview.TournamentOverviewDto? overview =
                await _tournamentService.GetTournamentOverviewAsync(tournamentId, userId);
            if (overview == null)
                return NotFound();

            return Ok(overview);
        }
    }
}
