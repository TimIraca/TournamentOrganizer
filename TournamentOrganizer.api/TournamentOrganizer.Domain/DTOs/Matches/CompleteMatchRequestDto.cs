using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs.Matches
{
    public class CompleteMatchRequest
    {
        public Guid WinnerId { get; set; }
    }
}
