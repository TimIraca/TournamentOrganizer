namespace TournamentOrganizer.api.DTOs
{
    public class TournamentApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public bool IsCompleted { get; set; }
        public IEnumerable<ParticipantApiDto> Participants { get; set; } =
            new List<ParticipantApiDto>();
    }
}
