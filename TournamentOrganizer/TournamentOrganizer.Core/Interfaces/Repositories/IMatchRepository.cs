﻿using TournamentOrganizer.Core.DTOs;

namespace TournamentOrganizer.Core.Interfaces.Repositories
{
    public interface IMatchRepository
    {
        Task<MatchCoreDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<MatchCoreDto>> GetAllByRoundIdAsync(Guid roundId);
        Task<MatchCoreDto> AddAsync(MatchCoreDto match);
        Task UpdateAsync(MatchCoreDto match);
        Task DeleteAsync(Guid id);
    }
}
