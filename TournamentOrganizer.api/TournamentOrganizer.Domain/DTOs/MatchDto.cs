using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs
{
    public class MatchDto
    {
        public Guid Id { get; set; }
        public int Round { get; set; }
        public ParticipantDto Participant1 { get; set; }
        public ParticipantDto Participant2 { get; set; }
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
        public string Status { get; set; }
        public DateTime? ScheduledTime { get; set; }
    }
}
