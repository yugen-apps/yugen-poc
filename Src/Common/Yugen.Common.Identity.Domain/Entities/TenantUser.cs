using System;
using Yugen.Common.Persistence.Entities;

namespace Yugen.Common.Identity.Domain.Entities;

public class TenantUser : BaseEntity
{
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public Guid? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
