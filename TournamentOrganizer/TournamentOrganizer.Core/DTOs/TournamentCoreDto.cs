using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.DAL.Entities;

namespace TournamentOrganizer.Core.DTOs
{
    public class TournamentCoreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<ParticipantCoreDto> Participants { get; set; } =
            new List<ParticipantCoreDto>();
        public IEnumerable<RoundCoreDto> Rounds { get; set; } = new List<RoundCoreDto>();
    }
}
