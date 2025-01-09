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

        public async Task<Participant?> GetByIdAsync(Guid id)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Participant>> GetAllByTournamentIdAsync(Guid tournamentId)
        {
            return await _context
                .Participants.Where(p => p.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<Participant> AddAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return participant;
        }

        public async Task UpdateAsync(Participant participant)
        {
            _context.Participants.Update(participant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Participant? participant = await GetByIdAsync(id);
            if (participant == null)
            {
                return;
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}
