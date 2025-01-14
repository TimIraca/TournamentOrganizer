using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Interfaces.Repositories;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class RoundRepository : IRoundRepository
    {
        private readonly TournamentContext _context;
        private readonly IMapper _mapper;

        public RoundRepository(TournamentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoundCoreDto?> GetByIdAsync(Guid id)
        {
            return await _context
                .Rounds.Include(r => r.Matches)
                .Where(r => r.Id == id)
                .Select(r => _mapper.Map<RoundCoreDto>(r))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RoundCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId)
        {
            return await _context
                .Rounds.Where(r => r.TournamentId == tournamentId)
                .Include(r => r.Matches)
                .OrderBy(r => r.RoundNumber)
                .Select(r => _mapper.Map<RoundCoreDto>(r))
                .ToListAsync();
        }

        public async Task<RoundCoreDto> AddAsync(RoundCoreDto round)
        {
            Round roundDAL = _mapper.Map<Round>(round);
            await _context.Rounds.AddAsync(roundDAL);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoundCoreDto>(round);
        }

        public async Task UpdateAsync(RoundCoreDto roundcore)
        {
            Round round = _mapper.Map<Round>(roundcore);
            foreach (Match match in round.Matches)
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Match> matchEntry =
                    _context.Entry(match);
                if (matchEntry.State == EntityState.Detached)
                {
                    Match? existingMatch = await _context.Matches.FindAsync(match.Id);
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
            Round? round = await _context.Rounds.FindAsync(id);
            if (round == null)
            {
                return;
            }

            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();
        }
    }
}
