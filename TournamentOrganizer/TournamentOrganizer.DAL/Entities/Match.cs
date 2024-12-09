using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.DAL.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public int MatchNumber { get; set; }
        public Guid RoundId { get; set; }

        // Foreign Keys
        public Guid? Participant1Id { get; set; }
        public Guid? Participant2Id { get; set; }
        public Guid? WinnerId { get; set; }

        // Navigation Properties
        public Round Round { get; set; } = null!;
        public Participant? Participant1 { get; set; }
        public Participant? Participant2 { get; set; }
        public Participant? Winner { get; set; }
    }
}
