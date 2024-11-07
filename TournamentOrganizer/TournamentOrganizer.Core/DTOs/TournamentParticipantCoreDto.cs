﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Core.DTOs
{
    public class TournamentParticipantCoreDto
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public string ParticipantName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}