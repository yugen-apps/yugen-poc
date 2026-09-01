using System;

namespace Yugen.Common.Blazor.Account.Features.Tenants.DTOs;

public class TenantDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}