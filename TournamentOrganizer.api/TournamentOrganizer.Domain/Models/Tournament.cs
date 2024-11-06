using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.Models
{
    public class Tournament
    {
        public Tournament()
        {
            // Initialize collections in constructor
            Participants = new List<TournamentParticipant>();
            Matches = new List<Match>();
            PrizeDistributions = new List<PrizeDistribution>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TournamentFormat Format { get; set; }
        public TournamentStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }

        // Navigation properties
        public List<TournamentParticipant> Participants { get; set; }
        public List<Match> Matches { get; set; }
        public List<PrizeDistribution> PrizeDistributions { get; set; }
    }
}
