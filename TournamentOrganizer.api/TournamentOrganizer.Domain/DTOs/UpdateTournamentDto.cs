using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs
{
    public class UpdateTournamentDto
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int MaxParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }
    }
}
