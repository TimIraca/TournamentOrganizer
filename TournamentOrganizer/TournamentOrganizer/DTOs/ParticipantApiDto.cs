namespace TournamentOrganizer.api.DTOs
{
    public class ParticipantApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid TournamentId { get; set; }
    }
}
