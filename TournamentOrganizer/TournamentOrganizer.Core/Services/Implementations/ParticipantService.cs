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

        public async Task<TournamentParticipantCoreDto> GetByIdAsync(Guid id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            return participant != null
                ? _mapper.Map<TournamentParticipantCoreDto>(participant)
                : null;
        }

        public async Task<IEnumerable<TournamentParticipantCoreDto>> GetAllByTournamentIdAsync(
            Guid tournamentId
        )
        {
            var participants = await _participantRepository.GetAllByTournamentIdAsync(tournamentId);
            return _mapper.Map<IEnumerable<TournamentParticipantCoreDto>>(participants);
        }

        public async Task AddAsync(TournamentParticipantCoreDto participantDto)
        {
            var currentCount = await _participantRepository.CountParticipantsAsync(
                participantDto.TournamentId
            );

            var tournament = await _participantRepository.GetTournamentAsync(
                participantDto.TournamentId
            );
            if (tournament == null)
            {
                throw new Exception("Tournament not found");
            }

            if (currentCount >= tournament.MaxParticipants)
            {
                throw new InvalidOperationException("Maximum number of participants reached.");
            }
            var participant = _mapper.Map<TournamentParticipant>(participantDto);
            await _participantRepository.AddAsync(participant);
        }

        public async Task UpdateAsync(TournamentParticipantCoreDto participantDto)
        {
            var participant = _mapper.Map<TournamentParticipant>(participantDto);
            await _participantRepository.UpdateAsync(participant);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _participantRepository.DeleteAsync(id);
        }
    }
}
