using Microsoft.AspNetCore.Identity;
using System;

namespace Yugen.Common.Identity.Domain.Entities;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser? User { get; set; }
    public virtual ApplicationRole? Role { get; set; }
}
