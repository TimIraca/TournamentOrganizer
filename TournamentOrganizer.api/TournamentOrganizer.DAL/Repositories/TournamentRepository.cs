using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentOrganizerContext _context;

        public TournamentRepository(TournamentOrganizerContext context)
        {
            _context = context;
        }

        public async Task<Tournament> GetTournamentByIdAsync(Guid id)
        {
            return await _context
                .Tournaments.Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tournament>> GetAllTournamentsAsync()
        {
            return await _context.Tournaments.ToListAsync();
        }

        public async Task CreateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Update(tournament);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTournamentAsync(Guid id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
            }
        }
    }
}
