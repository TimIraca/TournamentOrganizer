﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.core.DTOs
{
    public class LoginDto
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
