using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs.Requests;
using TournamentOrganizer.api.DTOs.Responses;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TournamentOrganizer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;

        public ParticipantController(IParticipantService participantService, IMapper mapper)
        {
            _participantService = participantService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentParticipantResponseApiDto>> GetById(Guid id)
        {
            var participantDto = await _participantService.GetByIdAsync(id);
            if (participantDto == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<TournamentParticipantResponseApiDto>(participantDto);
            return Ok(response);
        }

        [HttpGet("tournament/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<TournamentParticipantResponseApiDto>>> GetAllByTournamentId(Guid tournamentId)
        {
            var participantsDto = await _participantService.GetAllByTournamentIdAsync(tournamentId);
            var response = _mapper.Map<IEnumerable<TournamentParticipantResponseApiDto>>(participantsDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddParticipant(TournamentParticipantRequestApiDto request)
        {
            var participantDto = _mapper.Map<TournamentParticipantCoreDto>(request);
            await _participantService.AddAsync(participantDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateParticipant(Guid id, TournamentParticipantRequestApiDto request)
        {
            var participantDto = _mapper.Map<TournamentParticipantCoreDto>(request);
            participantDto.Id = id;

            await _participantService.UpdateAsync(participantDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParticipant(Guid id)
        {
            await _participantService.DeleteAsync(id);
            return NoContent();
        }
    }
}
