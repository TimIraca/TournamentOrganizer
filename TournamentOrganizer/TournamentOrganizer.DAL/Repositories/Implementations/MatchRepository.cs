using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class MatchRepository : IMatchRepository
    {
        private readonly TournamentContext _context;
        private readonly IMapper _mapper;

        public MatchRepository(TournamentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MatchCoreDto?> GetByIdAsync(Guid id)
        {
            var match = await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Include(m => m.Winner)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<MatchCoreDto?>(match);
        }

        public async Task<IEnumerable<MatchCoreDto>> GetAllByRoundIdAsync(Guid roundId)
        {
            var matches = await _context
                .Matches.Where(m => m.RoundId == roundId)
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Include(m => m.Winner)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MatchCoreDto>>(matches);
        }

        public async Task<MatchCoreDto> AddAsync(MatchCoreDto match)
        {
            await _context.Matches.AddAsync(_mapper.Map<Match>(match));
            await _context.SaveChangesAsync();
            MatchCoreDto matchDto = _mapper.Map<MatchCoreDto>(match);
            return matchDto;
        }

        public async Task UpdateAsync(MatchCoreDto match)
        {
            Match? existingMatch = await _context.Matches.FindAsync(match.Id);
            if (existingMatch != null)
            {
                existingMatch.WinnerId = match.WinnerId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            Match? match = await _context.Matches.FindAsync(id);
            if (match == null)
                return;

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }
}
