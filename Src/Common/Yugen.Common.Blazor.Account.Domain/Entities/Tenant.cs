using System.Collections.Generic;
using Yugen.Common.Data.Entities;

namespace Yugen.Common.Blazor.Account.Domain.Entities;

public class Tenant : BaseEntity
{
    public string? Name { get; set; }

    public ICollection<TenantUser> TenantUsers { get; set; } = [];
}
