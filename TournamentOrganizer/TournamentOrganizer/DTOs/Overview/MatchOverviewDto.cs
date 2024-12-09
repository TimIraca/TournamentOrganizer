namespace TournamentOrganizer.api.DTOs.Overview
{
    public class MatchOverviewDto
    {
        public Guid MatchId { get; set; }
        public int MatchNumber { get; set; }
        public string Participant1Name { get; set; } = string.Empty;
        public string Participant2Name { get; set; } = string.Empty;
        public string WinnerName { get; set; } = "TBD";
    }
}
