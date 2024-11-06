using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.Models
{
    public class PrizeDistribution
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public int Place { get; set; }
        public decimal Percentage { get; set; }

        public Tournament Tournament { get; set; }
    }
}
