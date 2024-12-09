namespace TournamentOrganizer.api.DTOs
{
    public class MatchApiDto
    {
        public Guid Id { get; set; }
        public int MatchNumber { get; set; }
        public Guid? Participant1Id { get; set; }
        public Guid? Participant2Id { get; set; }
        public Guid? WinnerId { get; set; }
    }
}
