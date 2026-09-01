using System.Collections.Generic;
using System.Linq;
using Riok.Mapperly.Abstractions;
using Yugen.Common.Identity.Core.Features.Tenants.DTOs;
using Yugen.Common.Identity.Core.Features.Tenants.Mappers;
using Yugen.Common.Identity.Core.Features.Users.DTOs;
using Yugen.Common.Identity.Domain.Entities;

namespace Yugen.Common.Identity.Core.Features.Users.Mappers;

[Mapper]
public static partial class UserMapper
{
    [MapperIgnoreSource(nameof(ApplicationUser.AccessFailedCount))]
    [MapperIgnoreSource(nameof(ApplicationUser.ConcurrencyStamp))]
    [MapperIgnoreSource(nameof(ApplicationUser.EmailConfirmed))]
    [MapperIgnoreSource(nameof(ApplicationUser.LockoutEnabled))]
    [MapperIgnoreSource(nameof(ApplicationUser.LockoutEnd))]
    [MapperIgnoreSource(nameof(ApplicationUser.NormalizedUserName))]
    [MapperIgnoreSource(nameof(ApplicationUser.NormalizedEmail))]
    [MapperIgnoreSource(nameof(ApplicationUser.PasswordHash))]
    [MapperIgnoreSource(nameof(ApplicationUser.PhoneNumber))]
    [MapperIgnoreSource(nameof(ApplicationUser.PhoneNumberConfirmed))]
    [MapperIgnoreSource(nameof(ApplicationUser.SecurityStamp))]
    [MapperIgnoreSource(nameof(ApplicationUser.TwoFactorEnabled))]
    [MapperIgnoreSource(nameof(ApplicationUser.TenantId))]
    [MapProperty(nameof(ApplicationUser.Tenant), nameof(UserDto.Tenant))]
    [MapProperty(nameof(ApplicationUser.TenantUsers), nameof(UserDto.AvailableTenants), Use = nameof(ToAvailableTenants))]
    public static partial UserDto ToUserDto(this ApplicationUser applicationUser);

    private static List<TenantDto> ToAvailableTenants(ICollection<TenantUser> tenantUsers)
    {
        return tenantUsers
            .Where(x => x.Tenant != null)
            .Select(x => x.Tenant!.ToTenantDto())
            .ToList();
    }

    private static TenantDto ToTenantDto(Tenant tenant) => tenant.ToTenantDto();
}