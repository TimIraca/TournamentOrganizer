using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TournamentOrganizer.api.DTOs;
    using TournamentOrganizer.Core.DTOs;

    [ApiController]
    [Route("api/tournaments/{tournamentId}/rounds")]
    public class RoundController : ControllerBase
    {
        private readonly IRoundService _roundService;
        private readonly IMapper _mapper;

        public RoundController(IRoundService roundService, IMapper mapper)
        {
            _roundService = roundService;
            _mapper = mapper;
        }

        // GET: api/rounds/tournament/{tournamentId}
        [HttpGet()]
        public async Task<IActionResult> GetRoundsByTournamentId(Guid tournamentId)
        {
            var coreDtos = await _roundService.GetAllByTournamentIdAsync(tournamentId);
            var apiDtos = _mapper.Map<IEnumerable<RoundApiDto>>(coreDtos);

            return Ok(apiDtos);
        }

        // GET: api/rounds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoundById(Guid id)
        {
            var coreDto = await _roundService.GetByIdAsync(id);
            if (coreDto == null)
                return NotFound();

            var apiDto = _mapper.Map<RoundApiDto>(coreDto);
            return Ok(apiDto);
        }

        // POST: api/rounds
        [HttpPost]
        public async Task<IActionResult> AddRound([FromBody] RoundApiDto apiDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coreDto = _mapper.Map<RoundCoreDto>(apiDto);
            var addedCoreDto = await _roundService.AddAsync(coreDto);

            var addedApiDto = _mapper.Map<RoundApiDto>(addedCoreDto);
            return CreatedAtAction(nameof(GetRoundById), new { id = addedApiDto.Id }, addedApiDto);
        }

        // PUT: api/rounds/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRound(Guid id, [FromBody] RoundApiDto apiDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != apiDto.Id)
                return BadRequest("ID mismatch.");

            var coreDto = _mapper.Map<RoundCoreDto>(apiDto);
            await _roundService.UpdateAsync(coreDto);

            return NoContent();
        }

        // DELETE: api/rounds/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRound(Guid id)
        {
            await _roundService.DeleteAsync(id);
            return NoContent();
        }
    }
}
