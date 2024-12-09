using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Core.DTOs
{
    public class RoundCoreDto
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public Guid TournamentId { get; set; }
        public IEnumerable<MatchCoreDto> Matches { get; set; } = new List<MatchCoreDto>();
    }
}
