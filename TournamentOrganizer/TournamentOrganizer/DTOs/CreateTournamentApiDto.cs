namespace TournamentOrganizer.api.DTOs
{
    public class CreateTournamentApiDto
    {
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
