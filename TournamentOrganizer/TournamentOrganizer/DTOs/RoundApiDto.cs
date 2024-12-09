namespace TournamentOrganizer.api.DTOs
{
    public class RoundApiDto
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public Guid TournamentId { get; set; }
    }
}
