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
        public Guid TournamentId { get; set; }
        public int Round { get; set; }
        public Guid? Participant1Id { get; set; }
        public Guid? Participant2Id { get; set; }
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
        public MatchStatus Status { get; set; }
        public Guid? WinnerId { get; set; }
        public DateTime? ScheduledTime { get; set; }

        public Tournament Tournament { get; set; }
        public TournamentParticipant Participant1 { get; set; }
        public TournamentParticipant Participant2 { get; set; }
    }
}
