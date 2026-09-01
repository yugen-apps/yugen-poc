using System;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Models;

namespace Yugen.Common.Blazor.Account.Features.Tenants.Services;

public interface ITenantService
{
    Task<Result<ApplicationUser>> SwitchToTenantAsync(Guid userId, Guid tenantId);
}