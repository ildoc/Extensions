using System.Collections.Generic;

namespace Infrastructure.Authorization
{
    public record UserInfo
    {
        public string UserId { get; init; }
        public string TaxCode { get; init; }
        public bool IsPhysicalPerson { get; init; }
        public string Email { get; init; }
        public bool IsImpersonated { get; init; }

        public IEnumerable<string> Roles { get; init; }
    }
}
