using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Poc.Identity.Domain.Entities;
using Poc.Identity.Domain.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Common.Identity.Domain.Entities;

namespace Poc.Identity.Persistence.Contexts;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ApplicationDbContextInitializer> _logger;

    public ApplicationDbContextInitializer(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<ApplicationDbContextInitializer> logger)
    {
        _logger = logger;
        _dbContext = dbContextFactory.CreateDbContext();
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        await _dbContext.MigrateAsync();
        await SeedDataAsync();
    }

    public async Task SeedDataAsync()
    {
        try
        {
            await SeedTenantsAsync();
            await SeedRolesAsync();
            var tenants = await _dbContext.Set<Tenant>().ToListAsync();
            await SeedUsersAsync(tenants);
            await SeedToDoItemsAsync(tenants);
            _dbContext.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task SeedTenantsAsync()
    {
        if (await _dbContext.Set<Tenant>().AnyAsync())
        {
            return;
        }

        _logger.LogInformation("Seeding tenants...");
        var tenants = new[]
        {
            new Tenant { Name = "Main" },
            new Tenant { Name = "Europe" }
        };

        await _dbContext.Set<Tenant>().AddRangeAsync(tenants);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        var superAdminRoleName = RoleConstants.SuperAdmin;
        var adminRoleName = RoleConstants.Admin;
        var userRoleName = RoleConstants.User;

        if (await _roleManager.RoleExistsAsync(superAdminRoleName))
        {
            return;
        }

        _logger.LogInformation("Seeding roles...");
        var superAdminRole = new ApplicationRole { Name = superAdminRoleName };
        var adminRole = new ApplicationRole { Name = adminRoleName };
        var userRole = new ApplicationRole { Name = userRoleName };

        await _roleManager.CreateAsync(superAdminRole);
        await _roleManager.CreateAsync(adminRole);
        await _roleManager.CreateAsync(userRole);
    }

    public async Task<IdentityResult?> SeedUsersAsync(List<Tenant> tenants)
    {
        if (await _userManager.Users.AnyAsync())
        {
            return null;
        }

        ApplicationUser user0 = new()
        {
            UserName = "user0@asd.com",
            TenantId = tenants[0].Id,
            Email = "user0@asd.com",
            EmailConfirmed = true,
            PhoneNumber = "123",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            TenantUsers = tenants.Select(t => new TenantUser { TenantId = t.Id }).ToList()
        };
        var identityResult = await _userManager.CreateAsync(user0, "Password123!");
        await _userManager.AddToRoleAsync(user0, RoleConstants.SuperAdmin);

        ApplicationUser user1 = new()
        {
            UserName = "user1@asd.com",
            TenantId = tenants[0].Id,
            Email = "user1@asd.com",
            EmailConfirmed = true,
            PhoneNumber = "123",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            TenantUsers = [new TenantUser { TenantId = tenants[0].Id }]
        };
        identityResult = await _userManager.CreateAsync(user1, "Password123!");
        await _userManager.AddToRoleAsync(user1, RoleConstants.User);

        ApplicationUser user2 = new()
        {
            UserName = "user2@asd.com",
            TenantId = tenants[1].Id,
            Email = "user2@asd.com",
            EmailConfirmed = true,
            PhoneNumber = "123",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            TenantUsers = [new TenantUser { TenantId = tenants[1].Id }]
        };
        identityResult = await _userManager.CreateAsync(user2, "Password123!");
        await _userManager.AddToRoleAsync(user1, RoleConstants.User);
        return identityResult;
    }

    public async Task SeedToDoItemsAsync(List<Tenant> tenants)
    {
        if (await _dbContext.Set<ToDoItem>().AnyAsync())
        {
            return;
        }

        ToDoItem[] toDoItems =
        [
            new ToDoItem { Title = "ToDoItem1", TenantId = tenants[0].Id },
            new ToDoItem { Title = "ToDoItem2", TenantId = tenants[1].Id }
        ];

        await _dbContext.Set<ToDoItem>().AddRangeAsync(toDoItems);
        await _dbContext.SaveChangesAsync();
    }
}