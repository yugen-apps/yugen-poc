using System.Linq;
using Riok.Mapperly.Abstractions;
using Yugen.Common.Identity.Core.Features.Tenants.DTOs;
using Yugen.Common.Identity.Domain.Entities;

namespace Yugen.Common.Identity.Core.Features.Tenants.Mappers;

[Mapper]
public static partial class TenantMapper
{
    [MapperIgnoreSource(nameof(Tenant.TenantUsers))]
    public static partial TenantDto ToTenantDto(this Tenant tenant);

    public static partial IQueryable<TenantDto> ToTenantsDto(this IQueryable<Tenant> toDoItem);
}