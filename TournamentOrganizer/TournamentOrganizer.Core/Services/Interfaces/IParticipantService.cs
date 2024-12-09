using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantCoreDto>> GetParticipantsByTournamentIdAsync(Guid tournamentId);
        Task<ParticipantCoreDto> AddParticipantAsync(ParticipantCoreDto participant);
        Task UpdateParticipantAsync(ParticipantCoreDto participant);
        Task DeleteParticipantAsync(Guid id);
    }
}
