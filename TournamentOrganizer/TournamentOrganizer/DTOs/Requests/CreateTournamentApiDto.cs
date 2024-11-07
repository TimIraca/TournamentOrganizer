using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.api.DTOs.Requests
{
    public class CreateTournamentApiDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }
        public TournamentFormat Format { get; set; }
    }
}
