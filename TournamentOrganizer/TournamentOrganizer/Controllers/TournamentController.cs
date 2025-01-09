using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TournamentOrganizer.api.DTOs;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

[ApiController]
[Route("api/tournaments")]
[Authorize]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _tournamentService;
    private readonly IMapper _mapper;

    public TournamentsController(ITournamentService tournamentService, IMapper mapper)
    {
        _tournamentService = tournamentService;
        _mapper = mapper;
    }

    private Guid GetCurrentUserId()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
        {
            throw new UnauthorizedAccessException("User not properly authenticated");
        }
        return userId;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTournaments()
    {
        var userId = GetCurrentUserId();
        IEnumerable<TournamentCoreDto> coreDtos = await _tournamentService.GetAllTournamentsAsync(
            userId
        );
        IEnumerable<TournamentApiDto> apiDtos = _mapper.Map<IEnumerable<TournamentApiDto>>(
            coreDtos
        );
        return Ok(apiDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTournamentById(Guid id)
    {
        var userId = GetCurrentUserId();
        TournamentCoreDto? coreDto = await _tournamentService.GetTournamentByIdAsync(id, userId);
        if (coreDto == null)
            return NotFound();
        TournamentApiDto apiDto = _mapper.Map<TournamentApiDto>(coreDto);
        return Ok(apiDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTournament([FromBody] CreateTournamentApiDto apiDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        TournamentCoreDto coreDto = _mapper.Map<TournamentCoreDto>(apiDto);
        coreDto.UserId = userId;

        TournamentCoreDto createdCoreDto = await _tournamentService.AddTournamentAsync(
            coreDto,
            userId
        );
        TournamentApiDto createdApiDto = _mapper.Map<TournamentApiDto>(createdCoreDto);

        return CreatedAtAction(
            nameof(GetTournamentById),
            new { id = createdApiDto.Id },
            createdApiDto
        );
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartTournament(Guid id)
    {
        var userId = GetCurrentUserId();
        TournamentCoreDto? coreDto = await _tournamentService.GetTournamentByIdAsync(id, userId);
        if (coreDto == null)
            return NotFound();

        await _tournamentService.StartTournamentAsync(id, userId);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTournament(
        Guid id,
        [FromBody] CreateTournamentApiDto apiDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        TournamentCoreDto coreDto = _mapper.Map<TournamentCoreDto>(apiDto);
        coreDto.Id = id;
        coreDto.UserId = userId;

        await _tournamentService.UpdateTournamentAsync(coreDto, userId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournament(Guid id)
    {
        var userId = GetCurrentUserId();
        await _tournamentService.DeleteTournamentAsync(id, userId);
        return NoContent();
    }
}
