﻿namespace TournamentOrganizer.Core.DTOs.Overview
{
    public class TournamentOverviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public bool IsCompleted { get; set; }
        public IEnumerable<RoundOverviewDto> Rounds { get; set; } = new List<RoundOverviewDto>();
        public IEnumerable<ParticipantOverviewDto> Participants { get; set; } =
            new List<ParticipantOverviewDto>();
    }
}
