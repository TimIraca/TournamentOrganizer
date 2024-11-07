namespace TournamentOrganizer.api.DTOs.Requests
{
    public class TournamentParticipantRequestApiDto
    {
        public Guid TournamentId { get; set; }
        public string ParticipantName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
