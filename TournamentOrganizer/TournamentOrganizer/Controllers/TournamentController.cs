// API Layer - Controllers/TournamentController.cs
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs.Requests;
using TournamentOrganizer.api.DTOs.Responses;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly IMapper _mapper;

        public TournamentController(ITournamentService tournamentService, IMapper mapper)
        {
            _tournamentService = tournamentService;
            _mapper = mapper;
        }

        // GET: api/Tournament
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentResponseApiDto>>> GetAllTournaments()
        {
            var tournaments = await _tournamentService.GetAllTournamentsAsync();
            return Ok(_mapper.Map<IEnumerable<TournamentResponseApiDto>>(tournaments));
        }

        // GET: api/Tournament/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentResponseApiDto>> GetTournamentById(Guid id)
        {
            var tournament = await _tournamentService.GetTournamentByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentResponseApiDto>(tournament));
        }

        // POST: api/Tournament
        [HttpPost]
        public async Task<ActionResult> CreateTournament(CreateTournamentApiDto tournamentDto)
        {
            TournamentCoreDto coreDto = _mapper.Map<TournamentCoreDto>(tournamentDto);
            await _tournamentService.AddTournamentAsync(coreDto);

            return CreatedAtAction(nameof(GetTournamentById), new { id = coreDto.Id }, tournamentDto);
        }

        // PUT: api/Tournament/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(Guid id, TournamentRequestApiDto tournamentDto)
        {
            var coreDto = _mapper.Map<TournamentCoreDto>(tournamentDto);
            coreDto.Id = id;
            await _tournamentService.UpdateTournamentAsync(coreDto);

            return NoContent();
        }

        // DELETE: api/Tournament/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            await _tournamentService.DeleteTournamentAsync(id);
            return NoContent();
        }
    }
}
