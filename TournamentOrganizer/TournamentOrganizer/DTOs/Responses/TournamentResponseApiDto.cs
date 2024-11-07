using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.api.DTOs.Responses
{
    public class TournamentResponseApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; }
        public string Status { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }
    }
}
