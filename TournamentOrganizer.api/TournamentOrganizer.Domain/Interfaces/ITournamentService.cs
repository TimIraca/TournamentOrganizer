using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.DTOs;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.Domain.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetAllAsync();
        Task<TournamentDto> GetByIdAsync(Guid id);
        Task<TournamentDto> CreateAsync(CreateTournamentDto createDto);
        Task<TournamentDto> UpdateAsync(Guid id, UpdateTournamentDto updateDto);
        Task<bool> DeleteAsync(Guid id);
        Task<ParticipantDto> RegisterParticipantAsync(
            Guid tournamentId,
            RegisterParticipantDto registerDto
        );
        Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId);
        Task<bool> StartTournamentAsync(Guid id);
        Task<bool> EndTournamentAsync(Guid id);
        Task<IEnumerable<ParticipantDto>> GetParticipantsAsync(Guid tournamentId);
        Task<IEnumerable<MatchDto>> GetMatchesAsync(Guid tournamentId);
        Task<bool> UpdateMatchScoreAsync(
            Guid tournamentId,
            Guid matchId,
            UpdateMatchScoreDto scoreDto
        );
    }
}
