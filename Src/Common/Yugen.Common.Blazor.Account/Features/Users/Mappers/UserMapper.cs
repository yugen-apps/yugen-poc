using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;

namespace Yugen.Common.Blazor.Account.Features.Users.Mappers;

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
    [MapperIgnoreSource(nameof(ApplicationUser.Tenant))]
    [MapperIgnoreSource(nameof(ApplicationUser.TenantId))]
    [MapperIgnoreSource(nameof(ApplicationUser.TenantUsers))]
    //[MapperIgnoreTarget(nameof(UserDto.AvailableTenants))]
    public static partial UserDto ToUserDto(this ApplicationUser applicationUser, TenantDto tenant, List<TenantDto> availableTenants);
}