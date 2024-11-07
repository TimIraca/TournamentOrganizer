namespace TournamentOrganizer.api.DTOs.Responses
{
    public class TournamentParticipantResponseApiDto
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string ParticipantName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
