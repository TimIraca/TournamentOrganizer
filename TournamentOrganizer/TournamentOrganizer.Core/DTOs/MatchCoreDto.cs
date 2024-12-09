using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Core.DTOs
{
    public class MatchCoreDto
    {
        public Guid Id { get; set; }
        public int MatchNumber { get; set; }
        public Guid? Participant1Id { get; set; }
        public Guid? Participant2Id { get; set; }
        public Guid? WinnerId { get; set; }
    }
}
