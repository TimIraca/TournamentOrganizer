using JwtAuth.core.Entities;

namespace JwtAuth.infrastructure.Entities
{
    public class Role : IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
