﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.core.Entities
{
    public interface IRole
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
