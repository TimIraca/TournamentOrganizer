using System.ComponentModel.DataAnnotations;

namespace TournamentOrganizer.api.DTOs
{
    public class AuthApiDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
