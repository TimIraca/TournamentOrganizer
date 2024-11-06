using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Domain.Interfaces;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.DAL.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentContext _context;

        public TournamentRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.PrizeDistributions)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Tournament> GetByIdAsync(Guid id)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.PrizeDistributions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tournament> GetByIdWithParticipantsAsync(Guid id)
        {
            // Use AsNoTracking to get a clean snapshot without change tracking
            return await _context
                .Tournaments.AsNoTracking()
                .Include(t => t.Participants)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tournament> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Matches)
                .Include(t => t.PrizeDistributions)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tournament> CreateAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
            return tournament;
        }

        public async Task UpdateAsync(Tournament tournament)
        {
            var existingTournament = await _context
                .Tournaments.Include(t => t.Participants)
                .Include(t => t.Matches)
                .Include(t => t.PrizeDistributions)
                .FirstOrDefaultAsync(t => t.Id == tournament.Id);

            if (existingTournament == null)
                return;

            // Update basic tournament properties
            _context.Entry(existingTournament).CurrentValues.SetValues(tournament);

            // Handle participant changes
            // Remove participants that exist in database but not in updated tournament
            foreach (var existingParticipant in existingTournament.Participants.ToList())
            {
                if (!tournament.Participants.Any(p => p.Id == existingParticipant.Id))
                {
                    existingTournament.Participants.Remove(existingParticipant);
                    _context.TournamentParticipants.Remove(existingParticipant);
                }
            }

            // Add new participants
            foreach (var participant in tournament.Participants)
            {
                var existingParticipant = existingTournament.Participants.FirstOrDefault(p =>
                    p.Id == participant.Id
                );

                if (existingParticipant == null)
                {
                    // First, add to the tournament's collection
                    existingTournament.Participants.Add(participant);

                    // Then explicitly tell EF this is a new entity
                    _context.Entry(participant).State = EntityState.Added;
                }
            }

            // Handle matches if they exist
            if (tournament.Matches != null)
            {
                foreach (var existingMatch in existingTournament.Matches.ToList())
                {
                    if (!tournament.Matches.Any(m => m.Id == existingMatch.Id))
                    {
                        existingTournament.Matches.Remove(existingMatch);
                        _context.Matches.Remove(existingMatch);
                    }
                }

                foreach (var match in tournament.Matches)
                {
                    if (!existingTournament.Matches.Any(m => m.Id == match.Id))
                    {
                        existingTournament.Matches.Add(match);
                        _context.Entry(match).State = EntityState.Added;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
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
