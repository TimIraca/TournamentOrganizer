using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.BLL.Services;
using TournamentOrganizer.Domain.DTOs;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAll()
        {
            var tournaments = await _tournamentService.GetAllAsync();
            return Ok(tournaments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetById(Guid id)
        {
            var tournament = await _tournamentService.GetByIdAsync(id);
            if (tournament == null)
                return NotFound();

            return Ok(tournament);
        }

        [HttpGet("{id}/participants")]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetParticipants(Guid id)
        {
            var participants = await _tournamentService.GetParticipantsAsync(id);
            if (participants == null)
                return NotFound();

            return Ok(participants);
        }

        [HttpGet("{id}/matches")]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatches(Guid id)
        {
            var matches = await _tournamentService.GetMatchesAsync(id);
            if (matches == null)
                return NotFound();

            return Ok(matches);
        }

        [HttpPost]
        public async Task<ActionResult<TournamentDto>> Create(CreateTournamentDto createDto)
        {
            var tournament = await _tournamentService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = tournament.Id }, tournament);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TournamentDto>> Update(
            Guid id,
            UpdateTournamentDto updateDto
        )
        {
            var tournament = await _tournamentService.UpdateAsync(id, updateDto);
            if (tournament == null)
                return NotFound();

            return Ok(tournament);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _tournamentService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/participants")]
        public async Task<ActionResult<ParticipantDto>> RegisterParticipant(
            Guid id,
            RegisterParticipantDto registerDto
        )
        {
            var participant = await _tournamentService.RegisterParticipantAsync(id, registerDto);
            if (participant == null)
                return BadRequest("Unable to register participant");

            return Ok(participant);
        }

        [HttpDelete("{id}/participants/{participantId}")]
        public async Task<ActionResult> RemoveParticipant(Guid id, Guid participantId)
        {
            var result = await _tournamentService.RemoveParticipantAsync(id, participantId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/start")]
        public async Task<ActionResult> StartTournament(Guid id)
        {
            var result = await _tournamentService.StartTournamentAsync(id);
            if (!result)
                return BadRequest("Unable to start tournament");

            return Ok();
        }

        [HttpPost("{id}/end")]
        public async Task<ActionResult> EndTournament(Guid id)
        {
            var result = await _tournamentService.EndTournamentAsync(id);
            if (!result)
                return BadRequest("Unable to end tournament");

            return Ok();
        }

        [HttpPut("{id}/matches/{matchId}/score")]
        public async Task<ActionResult> UpdateMatchScore(
            Guid id,
            Guid matchId,
            UpdateMatchScoreDto scoreDto
        )
        {
            var result = await _tournamentService.UpdateMatchScoreAsync(id, matchId, scoreDto);
            if (!result)
                return BadRequest("Unable to update match score");

            return Ok();
        }
    }
}
