using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;
using Yugen.Common.Blazor.Account.Features.Users.Mappers;

namespace Yugen.Common.Blazor.Account.Features.Users.Services;

public class UserService : IUserService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITenantService _tenantService;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IServiceScopeFactory serviceScopeFactory,
        ITenantService tenantService,
        ProtectedLocalStorage protectedLocalStorage,
        ILogger<UserService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _tenantService = tenantService;
        _protectedLocalStorage = protectedLocalStorage;
        _logger = logger;
    }

    public UserDto UserProfileState { get; private set; } = new();

    public event EventHandler<UserDto>? UserChanged;

    public async Task EnsureInitializedAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        //var tenantId = await GetTenantIdAsync();
        //UserProfileState.Tenant = UserProfileState.AvailableTenants.FirstOrDefault(x => x.Id == tenantId) ??
        //                          UserProfileState.AvailableTenants.FirstOrDefault(x => x.Id == applicationUser.TenantId) ??
        //                          UserProfileState.AvailableTenants.FirstOrDefault();
        //await SetTenantIdAsync(UserProfileState.Tenant?.Id);

        try
        {
            var applicationUser = await LoadUserProfileFromDatabaseAsync(userId, cancellationToken);
            var newUserProfileState = applicationUser?.ToUserDto() ?? new();
            SetInternal(newUserProfileState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize user profile for {UserId}", userId);
            throw;
        }
    }

    private void SetInternal(UserDto newUserProfileState)
    {
        if (UserProfileState != newUserProfileState)
        {
            UserProfileState = newUserProfileState;
            UserChanged?.Invoke(this, newUserProfileState);
        }
    }

    private async Task<ApplicationUser?> LoadUserProfileFromDatabaseAsync(Guid? userId, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        return await userManager.Users
            .Where(x => x.Id == userId)
            .Include(x => x.TenantUsers)
                .ThenInclude(x => x.Tenant)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task SetTenantIdAsync(Guid? tenantId)
    {
        if (tenantId == null)
        {
            return;
        }

        await _protectedLocalStorage.SetAsync(nameof(UserDto.Tenant), tenantId);
    }

    private async Task<Guid?> GetTenantIdAsync()
    {
        var result = await _protectedLocalStorage.GetAsync<Guid>(nameof(UserDto.Tenant));
        return result.Success ? result.Value : null;
    }
}