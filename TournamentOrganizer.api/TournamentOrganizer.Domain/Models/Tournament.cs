using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.Models
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RankingSystem RankingSystem { get; set; }
        public ICollection<Player> Players { get; set; }
    }

}
