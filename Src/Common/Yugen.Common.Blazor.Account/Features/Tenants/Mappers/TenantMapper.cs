using Riok.Mapperly.Abstractions;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Tenants.Mappers;

[Mapper]
public static partial class TenantMapper
{
    [MapperIgnoreSource(nameof(Tenant.TenantUsers))]
    public static partial TenantDto ToTenantDto(this Tenant tenant);

    // [MapperIgnoreTarget(nameof(Category.Id))]
    // public static partial Category ToCategory(this CreateCategoryInputDto category);

    // public static partial Category ToCategory(this UpdateCategoryInputDto category, int id);
}