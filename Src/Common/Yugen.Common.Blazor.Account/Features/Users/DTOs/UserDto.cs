using System;
using System.Collections.Generic;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Users.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }
    public string? Email { get; set; }

    public TenantDto? Tenant { get; set; }

    public List<TenantDto>? AvailableTenants { get; set; }
}