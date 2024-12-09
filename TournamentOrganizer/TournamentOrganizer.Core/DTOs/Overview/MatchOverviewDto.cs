namespace TournamentOrganizer.Core.DTOs.Overview
{
    public class MatchOverviewDto
    {
        public Guid Id { get; set; }
        public int MatchNumber { get; set; }
        public ParticipantOverviewDto? Participant1 { get; set; }
        public ParticipantOverviewDto? Participant2 { get; set; }
        public ParticipantOverviewDto? Winner { get; set; }
    }
}
