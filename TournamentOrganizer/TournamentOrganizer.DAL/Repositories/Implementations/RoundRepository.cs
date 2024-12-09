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
    public class RoundRepository : IRoundRepository
    {
        private readonly TournamentContext _context;

        public RoundRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<Round?> GetByIdAsync(Guid id)
        {
            return await _context
                .Rounds.Include(r => r.Matches)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Round>> GetAllByTournamentIdAsync(Guid tournamentId)
        {
            return await _context
                .Rounds.Where(r => r.TournamentId == tournamentId)
                .Include(r => r.Matches)
                .OrderBy(r => r.RoundNumber)
                .ToListAsync();
        }

        public async Task<Round> AddAsync(Round round)
        {
            await _context.Rounds.AddAsync(round);
            await _context.SaveChangesAsync();
            return round;
        }

        public async Task UpdateAsync(Round round)
        {
            foreach (var match in round.Matches)
            {
                var matchEntry = _context.Entry(match);
                if (matchEntry.State == EntityState.Detached)
                {
                    var existingMatch = await _context.Matches.FindAsync(match.Id);
                    if (existingMatch != null)
                    {
                        // Only update if values are different
                        if (existingMatch.WinnerId != match.WinnerId)
                        {
                            existingMatch.WinnerId = match.WinnerId;
                        }
                        if (existingMatch.Participant1Id != match.Participant1Id)
                        {
                            existingMatch.Participant1Id = match.Participant1Id;
                        }
                        if (existingMatch.Participant2Id != match.Participant2Id)
                        {
                            existingMatch.Participant2Id = match.Participant2Id;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var round = await GetByIdAsync(id);
            if (round == null)
                return;

            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();
        }
    }
}
