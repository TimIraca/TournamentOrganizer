using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Services;

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/tournaments/{tournamentId}/participants")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;

        public ParticipantsController(IParticipantService participantService, IMapper mapper)
        {
            _participantService = participantService;
            _mapper = mapper;
        }

        // GET: api/tournaments/{tournamentId}/participants
        [HttpGet]
        public async Task<IActionResult> GetParticipantsByTournamentId(Guid tournamentId)
        {
            IEnumerable<ParticipantCoreDto> coreDtos =
                await _participantService.GetParticipantsByTournamentIdAsync(tournamentId);
            IEnumerable<ParticipantApiDto> apiDtos = _mapper.Map<IEnumerable<ParticipantApiDto>>(
                coreDtos
            );
            return Ok(apiDtos);
        }

        // POST: api/tournaments/{tournamentId}/participants
        [HttpPost]
        [ProducesResponseType(typeof(ParticipantApiDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddParticipant(
            Guid tournamentId,
            [FromBody] CreateParticipantApiDto apiDto
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ParticipantCoreDto coreDto = _mapper.Map<ParticipantCoreDto>(apiDto);
            coreDto.TournamentId = tournamentId;

            ParticipantCoreDto createdCoreParticipantDto =
                await _participantService.AddParticipantAsync(coreDto);
            ParticipantApiDto createdApiParticipantDto = _mapper.Map<ParticipantApiDto>(
                createdCoreParticipantDto
            );

            return CreatedAtAction(
                nameof(GetParticipantsByTournamentId),
                new { tournamentId = createdApiParticipantDto.TournamentId },
                createdApiParticipantDto
            );
        }

        // PUT: api/tournaments/{tournamentId}/participants/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(
            Guid tournamentId,
            Guid id,
            [FromBody] CreateParticipantApiDto apiDto
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ParticipantCoreDto coreDto = _mapper.Map<ParticipantCoreDto>(apiDto);
            coreDto.TournamentId = tournamentId;
            coreDto.Id = id;
            await _participantService.UpdateParticipantAsync(coreDto);
            return NoContent();
        }

        // DELETE: api/tournaments/{tournamentId}/participants/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(Guid tournamentId, Guid id)
        {
            await _participantService.DeleteParticipantAsync(id);
            return NoContent();
        }
    }
}
