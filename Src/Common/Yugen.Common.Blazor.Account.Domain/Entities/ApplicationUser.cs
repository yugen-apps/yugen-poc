using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Yugen.Common.Blazor.Account.Domain.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? TenantId { get; set; }
    public virtual Tenant? Tenant { get; set; }

    public ICollection<TenantUser> TenantUsers { get; set; } = [];
}
