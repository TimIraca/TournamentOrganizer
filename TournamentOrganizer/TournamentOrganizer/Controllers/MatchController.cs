using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Implementations;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/tournaments/{tournamentId}/matches/")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;

        public MatchController(IMatchService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        [HttpGet("round/{roundId}")]
        public async Task<IActionResult> GetMatchesByRoundId(Guid roundId)
        {
            try
            {
                var matches = await _matchService.GetAllByRoundIdAsync(roundId);
                var apiDtos = _mapper.Map<IEnumerable<MatchApiDto>>(matches);
                return Ok(apiDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(Guid id)
        {
            try
            {
                var match = await _matchService.GetByIdAsync(id);
                var apiDto = _mapper.Map<MatchApiDto>(match);
                return Ok(apiDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMatch([FromBody] MatchApiDto apiDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var coreDto = _mapper.Map<MatchCoreDto>(apiDto);
                var addedMatch = await _matchService.AddAsync(coreDto);
                var addedApiDto = _mapper.Map<MatchApiDto>(addedMatch);

                return CreatedAtAction(
                    nameof(GetMatchById),
                    new { id = addedApiDto.Id },
                    addedApiDto
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(Guid id, [FromBody] MatchApiDto apiDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (id != apiDto.Id)
                    return BadRequest("ID mismatch between URL and body");

                var coreDto = _mapper.Map<MatchCoreDto>(apiDto);
                await _matchService.UpdateAsync(coreDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{matchId}/winner")]
        public async Task<IActionResult> DeclareWinner(
            Guid matchId,
            Guid tournamentId,
            [FromBody] DeclareWinnerRequestDto request
        )
        {
            try
            {
                await _matchService.DeclareMatchWinnerAsync(
                    tournamentId,
                    matchId,
                    request.WinnerId
                );
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
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(Guid id)
        {
            try
            {
                await _matchService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
