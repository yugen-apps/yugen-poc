using Riok.Mapperly.Abstractions;
using System.Linq;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Tenants.Mappers;

[Mapper]
public static partial class TenantMapper
{
    [MapperIgnoreSource(nameof(Tenant.TenantUsers))]
    public static partial TenantDto ToTenantDto(this Tenant tenant);

    public static partial IQueryable<TenantDto> ToTenantsDto(this IQueryable<Tenant> toDoItem);
}