using System;
using System.Threading.Tasks;
using Yugen.Common.Core.Models;
using Yugen.Common.Identity.Domain.Entities;

namespace Yugen.Common.Identity.Core.Features.Tenants.Services;

public interface ITenantService
{
    Task<Result<ApplicationUser>> SwitchToTenantAsync(Guid userId, Guid tenantId);
}