using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.DAL.Entities
{
    public class Participant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Foreign Key
        public Guid TournamentId { get; set; }

        // Navigation Properties
        public Tournament Tournament { get; set; } = null!;
        public ICollection<Match> MatchesAsParticipant1 { get; set; } = new List<Match>();
        public ICollection<Match> MatchesAsParticipant2 { get; set; } = new List<Match>();
    }
}
