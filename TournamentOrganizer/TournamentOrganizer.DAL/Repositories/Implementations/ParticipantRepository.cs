using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly TournamentContext _context;

        public ParticipantRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<TournamentParticipant> GetByIdAsync(Guid id)
        {
            return await _context
                .TournamentParticipants.Include(p => p.Tournament)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<TournamentParticipant>> GetAllByTournamentIdAsync(
            Guid tournamentId
        )
        {
            return await _context
                .TournamentParticipants.Where(p => p.TournamentId == tournamentId)
                .Include(p => p.Tournament)
                .ToListAsync();
        }

        public async Task AddAsync(TournamentParticipant participant)
        {
            await _context.TournamentParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TournamentParticipant participant)
        {
            _context.TournamentParticipants.Update(participant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var participant = await _context.TournamentParticipants.FindAsync(id);
            if (participant != null)
            {
                _context.TournamentParticipants.Remove(participant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountParticipantsAsync(Guid tournamentId)
        {
            return await _context.TournamentParticipants.CountAsync(p =>
                p.TournamentId == tournamentId
            );
        }

        public async Task<Tournament> GetTournamentAsync(Guid tournamentId)
        {
            return await _context.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);
        }
    }
}
