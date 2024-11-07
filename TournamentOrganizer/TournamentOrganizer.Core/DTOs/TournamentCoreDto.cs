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
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TournamentFormat Format { get; set; }
        public TournamentStatus Status { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public decimal PrizePool { get; set; }
        public string PrizeCurrency { get; set; }
    }
}
