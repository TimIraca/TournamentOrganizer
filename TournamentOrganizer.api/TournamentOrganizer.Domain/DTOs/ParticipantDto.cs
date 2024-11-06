﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Domain.DTOs
{
    public class ParticipantDto
    {
        public Guid Id { get; set; }
        public string ParticipantName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
