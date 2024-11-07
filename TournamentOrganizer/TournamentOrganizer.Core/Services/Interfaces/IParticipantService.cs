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
        Task<TournamentParticipantCoreDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TournamentParticipantCoreDto>> GetAllByTournamentIdAsync(Guid tournamentId);
        Task AddAsync(TournamentParticipantCoreDto participantDto);
        Task UpdateAsync(TournamentParticipantCoreDto participantDto);
        Task DeleteAsync(Guid id);
    }
}
