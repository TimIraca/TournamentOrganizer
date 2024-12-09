using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.DAL.Entities
{
    public class Round
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }

        // Foreign Key
        public Guid TournamentId { get; set; }

        // Navigation Properties
        public Tournament Tournament { get; set; } = null!;
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
