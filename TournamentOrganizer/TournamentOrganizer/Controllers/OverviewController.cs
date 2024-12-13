using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OverviewController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public OverviewController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        // GET: api/overview/tournament/{tournamentId}
        [HttpGet("tournament/{tournamentId}")]
        public async Task<IActionResult> GetTournamentOverview(Guid tournamentId)
        {
            Core.DTOs.Overview.TournamentOverviewDto? overview = await _tournamentService.GetTournamentOverviewAsync(tournamentId);
            if (overview == null)
                return NotFound();

            return Ok(overview);
        }
    }
}
