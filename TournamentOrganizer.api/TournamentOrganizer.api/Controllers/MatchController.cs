using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.Domain;
using TournamentOrganizer.Domain.DTOs.Matches;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatch(Guid id)
        {
            try
            {
                var match = await _matchService.GetMatchAsync(id);
                return Ok(match);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("tournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<Match>>> GetTournamentMatches(Guid tournamentId)
        {
            var matches = await _matchService.GetTournamentMatchesAsync(tournamentId);
            return Ok(matches);
        }

        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch(Match match)
        {
            try
            {
                var createdMatch = await _matchService.CreateMatchAsync(match);
                return CreatedAtAction(
                    nameof(GetMatch),
                    new { id = createdMatch.Id },
                    createdMatch
                );
            }
            catch (TournamentOrganizer.Domain.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Match>> UpdateMatch(Guid id, Match match)
        {
            try
            {
                var updatedMatch = await _matchService.UpdateMatchAsync(id, match);
                return Ok(updatedMatch);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TournamentOrganizer.Domain.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/score")]
        public async Task<ActionResult<Match>> UpdateScore(
            Guid id,
            [FromBody] UpdateScoreRequestDto request
        )
        {
            try
            {
                var updatedMatch = await _matchService.UpdateScoreAsync(
                    id,
                    request.Participant1Score,
                    request.Participant2Score
                );
                return Ok(updatedMatch);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<Match>> CompleteMatch(
            Guid id,
            [FromBody] CompleteMatchRequest request
        )
        {
            try
            {
                var completedMatch = await _matchService.CompleteMatchAsync(id, request.WinnerId);
                return Ok(completedMatch);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (TournamentOrganizer.Domain.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(Guid id)
        {
            try
            {
                await _matchService.DeleteMatchAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("tournament/{tournamentId}/generate-bracket")]
        public async Task<ActionResult<IEnumerable<Match>>> GenerateBracket(
            Guid tournamentId,
            [FromBody] List<TournamentParticipant> participants
        )
        {
            try
            {
                var matches = await _matchService.GenerateSingleEliminationBracketAsync(
                    tournamentId,
                    participants
                );
                return Ok(matches);
            }
            catch (TournamentOrganizer.Domain.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("tournament/{tournamentId}/bracket")]
        public async Task<ActionResult<IEnumerable<Match>>> GetBracket(Guid tournamentId)
        {
            var bracket = await _matchService.GetBracketStructureAsync(tournamentId);
            return Ok(bracket);
        }

        [HttpPost("{matchId}/advance-winner")]
        public async Task<ActionResult> AdvanceWinner(Guid matchId)
        {
            try
            {
                await _matchService.AdvanceWinnerAsync(matchId);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
