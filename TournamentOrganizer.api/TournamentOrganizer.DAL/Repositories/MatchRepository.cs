using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly TournamentContext _context;

        public MatchRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<Match> GetByIdAsync(Guid id)
        {
            return await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetByTournamentIdAsync(Guid tournamentId)
        {
            return await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Where(m => m.TournamentId == tournamentId)
                .OrderBy(m => m.Round)
                .ToListAsync();
        }

        public async Task<IEnumerable<Match>> GetByRoundAsync(Guid tournamentId, int round)
        {
            return await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Where(m => m.TournamentId == tournamentId && m.Round == round)
                .ToListAsync();
        }

        public async Task<Match> CreateAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<Match> UpdateAsync(Match match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task DeleteAsync(Guid id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Match>> GetPendingMatchesAsync()
        {
            return await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Where(m => m.Status == MatchStatus.Pending)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Matches.AnyAsync(m => m.Id == id);
        }
    }
}
