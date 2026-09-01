using System.Collections.Generic;
using Yugen.Common.Persistence.Entities;

namespace Yugen.Common.Identity.Domain.Entities;

public class Tenant : BaseEntity
{
    public string? Name { get; set; }

    public ICollection<TenantUser> TenantUsers { get; set; } = [];
}
