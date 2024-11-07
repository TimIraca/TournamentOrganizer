using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentOrganizer.Domain.Models;

namespace TournamentOrganizer.Domain.DTOs
{
    public class CreateTournamentDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public TournamentFormat Format { get; set; }
        public DateTime StartDate { get; set; }
        public int MaxParticipants { get; set; }
        public decimal? PrizePool { get; set; }
        public string? PrizeCurrency { get; set; }
    }
}
