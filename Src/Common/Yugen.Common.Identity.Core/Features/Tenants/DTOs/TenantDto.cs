using System;

namespace Yugen.Common.Identity.Core.Features.Tenants.DTOs;

public class TenantDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}