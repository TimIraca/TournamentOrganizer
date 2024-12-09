namespace TournamentOrganizer.Core.DTOs.Overview
{
    public class RoundOverviewDto
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public IEnumerable<MatchOverviewDto> Matches { get; set; } = new List<MatchOverviewDto>();
    }
}
