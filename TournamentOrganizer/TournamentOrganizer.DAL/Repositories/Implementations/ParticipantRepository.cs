using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.DAL.Repositories.Implementations
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly TournamentContext _context;
        private readonly IMapper _mapper;

        public ParticipantRepository(TournamentContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParticipantCoreDto?> GetByIdAsync(Guid id)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<ParticipantCoreDto?>(participant);
        }

        public async Task<IEnumerable<ParticipantCoreDto>> GetAllByTournamentIdAsync(
            Guid tournamentId
        )
        {
            var participants = await _context
                .Participants.Where(p => p.TournamentId == tournamentId)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ParticipantCoreDto>>(participants);
        }

        public async Task<ParticipantCoreDto> AddAsync(ParticipantCoreDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return _mapper.Map<ParticipantCoreDto>(participant);
        }

        public async Task UpdateAsync(ParticipantCoreDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            _context.Participants.Update(participant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Id == id);
            if (participant == null)
            {
                return;
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}
