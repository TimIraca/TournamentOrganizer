using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournamentOrganizer.DAL.Entities
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }

        // Navigation Properties
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public ICollection<Round> Rounds { get; set; } = new List<Round>();
    }
}
