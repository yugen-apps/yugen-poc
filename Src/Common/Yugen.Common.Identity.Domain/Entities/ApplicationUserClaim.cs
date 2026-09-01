using Microsoft.AspNetCore.Identity;
using System;

namespace Yugen.Common.Identity.Domain.Entities;

public class ApplicationUserClaim : IdentityUserClaim<Guid>
{
    public virtual ApplicationUser? User { get; set; }
}
