namespace TournamentOrganizer.api.DTOs.Overview
{
    public class RoundOverviewDto
    {
        public Guid RoundId { get; set; }
        public int RoundNumber { get; set; }
        public IEnumerable<MatchOverviewDto> Matches { get; set; } = new List<MatchOverviewDto>();
    }

}
