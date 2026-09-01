using Microsoft.AspNetCore.Identity;
using System;

namespace Yugen.Common.Identity.Domain.Entities;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public virtual ApplicationUser? User { get; set; }
}