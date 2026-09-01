using Microsoft.AspNetCore.Identity;
using System;

namespace Yugen.Common.Identity.Domain.Entities;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ApplicationRole? Role { get; set; }
}
