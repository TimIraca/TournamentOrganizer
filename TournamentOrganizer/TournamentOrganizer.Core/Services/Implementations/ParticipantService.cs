using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TournamentOrganizer.Core.DTOs;
using TournamentOrganizer.Core.Services.Interfaces;
using TournamentOrganizer.DAL.Entities;
using TournamentOrganizer.DAL.Repositories.Interfaces;

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
            var participants = await _participantRepository.GetAllByTournamentIdAsync(tournamentId);
            return _mapper.Map<IEnumerable<ParticipantCoreDto>>(participants);
        }

        public async Task<ParticipantCoreDto> AddParticipantAsync(ParticipantCoreDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            Participant CreatedParticipant = await _participantRepository.AddAsync(participant);
            return _mapper.Map<ParticipantCoreDto>(CreatedParticipant);
        }

        public async Task UpdateParticipantAsync(ParticipantCoreDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            await _participantRepository.UpdateAsync(participant);
        }

        public async Task DeleteParticipantAsync(Guid id)
        {
            await _participantRepository.DeleteAsync(id);
        }
    }
}
