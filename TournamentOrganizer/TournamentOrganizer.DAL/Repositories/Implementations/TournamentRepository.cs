using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentContext _context;

        public TournamentRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<Tournament?> GetByIdAsync(Guid id, Guid userId)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Rounds)
                .ThenInclude(r => r.Matches)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync(Guid userId)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Rounds)
                .ThenInclude(r => r.Matches)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<Tournament> AddAsync(Tournament tournament)
        {
            await _context.Tournaments.AddAsync(tournament);
            await _context.SaveChangesAsync();
            return tournament;
        }

        public async Task UpdateAsync(Tournament tournament, Guid userId)
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
