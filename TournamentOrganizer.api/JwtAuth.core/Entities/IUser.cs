using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuth.core.Entities
{
    public interface IUser
    {
        string Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
    }
}
