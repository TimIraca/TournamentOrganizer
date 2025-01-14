using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;

namespace TournamentOrganizer.Core.Services.Implementations
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IMapper _mapper;

        public ParticipantService(IParticipantRepository participantRepository, IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipantCoreDto>> GetParticipantsByTournamentIdAsync(
            Guid tournamentId
        )
        {
            IEnumerable<ParticipantCoreDto> participants =
                await _participantRepository.GetAllByTournamentIdAsync(tournamentId);
            return _mapper.Map<IEnumerable<ParticipantCoreDto>>(participants);
        }

        public async Task<ParticipantCoreDto> AddParticipantAsync(ParticipantCoreDto participantDto)
        {
            ParticipantCoreDto participant = _mapper.Map<ParticipantCoreDto>(participantDto);
            ParticipantCoreDto CreatedParticipant = await _participantRepository.AddAsync(
                participant
            );
            return _mapper.Map<ParticipantCoreDto>(CreatedParticipant);
        }

        public async Task UpdateParticipantAsync(ParticipantCoreDto participantDto)
        {
            ParticipantCoreDto participant = _mapper.Map<ParticipantCoreDto>(participantDto);
            await _participantRepository.UpdateAsync(participant);
        }

        public async Task DeleteParticipantAsync(Guid id)
        {
            await _participantRepository.DeleteAsync(id);
        }
    }
}
