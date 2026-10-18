using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Data;

namespace Yugen.Common.Blazor.Account.Features.Tenants.Services;

public class TenantService : ITenantService
{
    private readonly IApplicationDbContextFactory _dbContextFactory;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TenantService> _logger;

    public TenantService(
        IApplicationDbContextFactory dbContextFactory,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<TenantService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task<Result<ApplicationUser>> SwitchToTenantAsync(Guid userId, Guid tenantId)
    {
        try
        {
            await using var dbContext = await _dbContextFactory.CreateAsync();

            // Get user and tenant information
            using var scope = _serviceScopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(userId.ToString());
            var tenant = await dbContext.Set<Tenant>().FindAsync(tenantId);
            if (user == null ||
                tenant == null)
            {
                return Result.Failure<ApplicationUser>("User or tenant not found");
            }

            // Record the original tenant ID for logging
            var originalTenantId = user.TenantId;

            // Update user's tenant ID
            user.TenantId = tenantId;

            // Update database
            await userManager.UpdateAsync(user);

            // Refresh user state and cache
            // await _userService.RefreshAsync();

            // Clear user context cache
            //_userContextLoader.ClearUserContextCache(userId);
            // Update user claims
            //await RefreshUserClaimsAsync(user, userManager);

            // Log successful tenant switch
            _logger.LogInformation("User {UserId} ({UserName}) successfully switched from tenant {OriginalTenantId} to tenant {NewTenantId} ({TenantName})",
                userId, user.UserName, originalTenantId, tenantId, tenant.Name);

            // Record switch result
            var result = Result.Success(user);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to switch user {UserId} to tenant {TenantId}", userId, tenantId);
            return Result.Failure<ApplicationUser>("Failed to switch tenant");
        }
    }
}