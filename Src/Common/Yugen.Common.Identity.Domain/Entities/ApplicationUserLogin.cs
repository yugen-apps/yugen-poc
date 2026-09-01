using Microsoft.AspNetCore.Identity;
using System;

namespace Yugen.Common.Identity.Domain.Entities;

public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    public virtual ApplicationUser? User { get; set; }
}
