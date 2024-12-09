using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

[ApiController]
[Route("api/tournaments")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    public TournamentsController(ITournamentService tournamentService, IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    // GET: api/tournaments
    [HttpGet]
    public async Task<IActionResult> GetAllTournaments()
    {
        var coreDtos = await _tournamentService.GetAllTournamentsAsync();
        var apiDtos = _mapper.Map<IEnumerable<TournamentApiDto>>(coreDtos);

        return Ok(apiDtos);
    }

    // GET: api/tournaments/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTournamentById(Guid id)
    {
        var coreDto = await _tournamentService.GetTournamentByIdAsync(id);
        if (coreDto == null)
            return NotFound();

        var apiDto = _mapper.Map<TournamentApiDto>(coreDto);
        return Ok(apiDto);
    }

    // POST: api/tournaments
    [HttpPost]
    public async Task<IActionResult> CreateTournament([FromBody] CreateTournamentApiDto apiDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var coreDto = _mapper.Map<TournamentCoreDto>(apiDto);

        // Add the tournament and get the generated ID
        var createdCoreDto = await _tournamentService.AddTournamentAsync(coreDto);

        // Map back to API DTO
        var createdApiDto = _mapper.Map<TournamentApiDto>(createdCoreDto);

        return CreatedAtAction(
            nameof(GetTournamentById),
            new { id = createdApiDto.Id },
            createdApiDto
        );
    }

    // POST: api/tournaments/{id}/start
    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartTournament(Guid id)
    {
        var coreDto = await _tournamentService.GetTournamentByIdAsync(id);
        if (coreDto == null)
            return NotFound();

        await _tournamentService.StartTournamentAsync(id);
        return Ok();
    }

    // PUT: api/tournaments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTournament(
        Guid id,
        [FromBody] CreateTournamentApiDto apiDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        //if (id != apiDto.Id)
        //    return BadRequest("ID mismatch.");
        var coreDto = _mapper.Map<TournamentCoreDto>(apiDto);
        coreDto.Id = id;
        await _tournamentService.UpdateTournamentAsync(coreDto);

        return NoContent();
    }

    // DELETE: api/tournaments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(Guid id)
    {
        await _tournamentService.DeleteTournamentAsync(id);
        return NoContent();
    }
}
