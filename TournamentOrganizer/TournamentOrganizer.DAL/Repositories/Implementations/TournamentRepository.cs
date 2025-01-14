using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentContext _context;
        private readonly IMapper _mapper;

        public TournamentRepository(TournamentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TournamentCoreDto?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Rounds)
                .ThenInclude(r => r.Matches)
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => _mapper.Map<TournamentCoreDto>(t))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TournamentCoreDto>> GetAllAsync(Guid userId)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Rounds)
                .ThenInclude(r => r.Matches)
                .Where(t => t.UserId == userId)
                .Select(t => _mapper.Map<TournamentCoreDto>(t))
                .ToListAsync();
        }

        public async Task<TournamentCoreDto> AddAsync(TournamentCoreDto tournament)
        {
            Tournament tournamentEntity = _mapper.Map<Tournament>(tournament);
            await _context.Tournaments.AddAsync(tournamentEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TournamentCoreDto>(tournamentEntity);
        }

        public async Task UpdateAsync(TournamentCoreDto tournament, Guid userId)
        {
            Tournament? existingTournament = await _context.Tournaments.FirstOrDefaultAsync(t =>
                t.Id == tournament.Id && t.UserId == userId
            );

            if (existingTournament != null)
            {
                // Update specific fields
                existingTournament.Name = tournament.Name;
                existingTournament.StartDate = tournament.StartDate;
                // Save changes
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            Tournament? tournament = await _context.Tournaments.FirstOrDefaultAsync(t =>
                t.Id == id && t.UserId == userId
            );

            if (tournament == null)
                return;

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
        }
    }
}
