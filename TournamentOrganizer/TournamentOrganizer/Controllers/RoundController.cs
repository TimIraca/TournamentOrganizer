using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace TournamentOrganizer.api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using TournamentOrganizer.api.DTOs;
    using TournamentOrganizer.Core.DTOs;
    using TournamentOrganizer.Core.Interfaces.Services;

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
            IEnumerable<RoundCoreDto> coreDtos = await _roundService.GetAllByTournamentIdAsync(
                tournamentId
            );
            IEnumerable<RoundApiDto> apiDtos = _mapper.Map<IEnumerable<RoundApiDto>>(coreDtos);

            return Ok(apiDtos);
        }

        // GET: api/rounds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoundById(Guid id)
        {
            RoundCoreDto? coreDto = await _roundService.GetByIdAsync(id);
            if (coreDto == null)
                return NotFound();

            RoundApiDto apiDto = _mapper.Map<RoundApiDto>(coreDto);
            return Ok(apiDto);
        }

        // POST: api/rounds
        [HttpPost]
        public async Task<IActionResult> AddRound([FromBody] RoundApiDto apiDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            RoundCoreDto coreDto = _mapper.Map<RoundCoreDto>(apiDto);
            RoundCoreDto addedCoreDto = await _roundService.AddAsync(coreDto);

            RoundApiDto addedApiDto = _mapper.Map<RoundApiDto>(addedCoreDto);
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

            RoundCoreDto coreDto = _mapper.Map<RoundCoreDto>(apiDto);
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
