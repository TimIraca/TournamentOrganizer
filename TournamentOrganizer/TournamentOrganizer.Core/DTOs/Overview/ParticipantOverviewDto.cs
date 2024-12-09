﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentOrganizer.Core.DTOs.Overview
{
    public class ParticipantOverviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
