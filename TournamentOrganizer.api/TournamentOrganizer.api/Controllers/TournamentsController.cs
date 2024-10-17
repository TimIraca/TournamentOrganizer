using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.BLL.Services;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly TournamentService _tournamentService;

        public TournamentsController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTournaments()
        {
            var tournaments = await _tournamentService.GetAllTournamentsAsync();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournamentById(Guid id)
        {
            var tournament = await _tournamentService.GetTournamentByIdAsync(id);
            if (tournament == null) return NotFound();
            return Ok(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTournament(Tournament tournament)
        {
            await _tournamentService.CreateTournamentAsync(tournament);
            return CreatedAtAction(nameof(GetTournamentById), new { id = tournament.Id }, tournament);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(Guid id, Tournament tournament)
        {
            if (id != tournament.Id) return BadRequest();
            await _tournamentService.UpdateTournamentAsync(tournament);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            await _tournamentService.DeleteTournamentAsync(id);
            return NoContent();
        }
    }
}
