using JwtAuth.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IUser user);
        bool ValidateToken(string token);
    }
}
