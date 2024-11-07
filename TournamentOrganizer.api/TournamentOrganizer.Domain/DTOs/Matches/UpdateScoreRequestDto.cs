using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs.Matches
{
    public class UpdateScoreRequestDto
    {
        public int? Participant1Score { get; set; }
        public int? Participant2Score { get; set; }
    }
}
