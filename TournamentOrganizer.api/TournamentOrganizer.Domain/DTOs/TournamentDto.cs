using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs
{
    public class TournamentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Format { get; set; } // Enum as string for frontend
        public string Status { get; set; } // Enum as string for frontend
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }
    }
}
