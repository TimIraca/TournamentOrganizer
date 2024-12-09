using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class MatchRepository : IMatchRepository
    {
        private readonly TournamentContext _context;

        public MatchRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<Match?> GetByIdAsync(Guid id)
        {
            return await _context
                .Matches.Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Include(m => m.Winner)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetAllByRoundIdAsync(Guid roundId)
        {
            return await _context
                .Matches.Where(m => m.RoundId == roundId)
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Include(m => m.Winner)
                .ToListAsync();
        }

        public async Task<Match> AddAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task UpdateAsync(Match match)
        {
            var existingMatch = await _context.Matches.FindAsync(match.Id);
            if (existingMatch != null)
            {
                existingMatch.WinnerId = match.WinnerId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            Match? match = await GetByIdAsync(id);
            if (match == null)
                return;

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }
    }
}
