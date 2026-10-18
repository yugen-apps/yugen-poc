using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Auth;
using Yugen.Common.Blazor.Account.Features.Tenants.Mappers;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;
using Yugen.Common.Blazor.Account.Features.Users.Mappers;

namespace Poc.Identity.Blazor.Services.User;

public class UserService : IUserService
{
    private readonly IAuthService _authService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITenantService _tenantService;
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    public UserService(
        IAuthService authService,
        IServiceScopeFactory serviceScopeFactory,
        ITenantService tenantService,
        ProtectedLocalStorage protectedLocalStorage)
    {
        _authService = authService;
        _serviceScopeFactory = serviceScopeFactory;
        _tenantService = tenantService;
        _protectedLocalStorage = protectedLocalStorage;
    }

    public UserDto UserProfileState { get; set; } = new();

    public event EventHandler? UserChanged;

    public async Task InitializedAsync()
    {
        var user = await _authService.GetCurrentUserAsync();
        var userId = user?.Id;
        if (userId == null)
        {
            return;
        }

        //var tenantsResult = await _handler.Handle(new GetAllTenantsQuery(), CancellationToken.None);
        //if (!tenantsResult.IsSuccess)
        //{
        //    return;
        //}

        //UserProfileState.AvailableTenants = tenantsResult.Value;

        //if (UserProfileState.Tenant != null)
        //{
        //    return;
        //}

        //var tenantId = await GetTenantIdAsync();
        //UserProfileState.Tenant = UserProfileState.AvailableTenants.FirstOrDefault(x => x.Id == tenantId) ??
        //                     UserProfileState.AvailableTenants.FirstOrDefault();

        using var scope = _serviceScopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var applicationUser = await userManager.Users
            .Where(x => x.Id == userId)
            .Include(x => x.TenantUsers)
                .ThenInclude(x => x.Tenant)
            .FirstOrDefaultAsync();
        if (applicationUser == null)
        {
            return;
        }

        UserProfileState = applicationUser.ToUserDto(applicationUser.Tenant.ToTenantDto(), applicationUser.TenantUsers.Select(x =>x.Tenant.ToTenantDto()).ToList());
        await SetTenantIdAsync(UserProfileState.Tenant.Id);

        UserChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task SetTenantIdAsync(Guid tenantId)
    {
        await _protectedLocalStorage.SetAsync(nameof(UserDto.Tenant), tenantId);
    }

    private async Task<Guid?> GetTenantIdAsync()
    {
        var result = await _protectedLocalStorage.GetAsync<Guid>(nameof(UserDto.Tenant));
        return result.Success ? result.Value : null;
    }
}