using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Identity.Blazor.Components;
using Poc.Identity.Blazor.Services.User;
using Poc.Identity.Domain.Entities;
using Poc.Identity.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Common;
using Yugen.Common.Blazor.Account;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Auth;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Models;
using Yugen.Common.Data;
using Yugen.Common.Data.Extensions;
using Yugen.Common.Web;

namespace Poc.Identity.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddScoped<SystemInfoService>();
        services.AddScoped<WebHostInfoService>();
        return services;
    }

    public static IServiceCollection AddIdentity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCascadingAuthenticationState();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        services.AddTransient<ITenantService, TenantService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddGoogle(options =>
        {
            options.ClientId = configuration.GetValue<string>("Authentication:Google:ClientId") ?? string.Empty;
            options.ClientSecret = configuration.GetValue<string>("Authentication:Google:ClientSecret") ?? string.Empty;
            options.ClaimActions.MapJsonKey(GoogleClaimTypes.Picture, "picture");
        })
        .AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();
        return services;
    }

    public static WebApplication ConfigureServer(
        this WebApplication app, 
        IConfiguration configuration)
    {
        // app.MapHealthChecks("/healthz");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies([
                typeof(Yugen.Common.Blazor._Imports).Assembly,
                typeof(Yugen.Common.Blazor.Account._Imports).Assembly
            ]);

        // CancellationTokenSource cancellation = new();
        // app.Lifetime.ApplicationStopping.Register(() =>
        // {
        //     cancellation.Cancel();
        // });

        //app.MapGet("/Environment", () =>
        //{
        //    return new EnvironmentInfo();
        //});

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        // This API demonstrates how to use task cancellation
        // to support graceful container shutdown via SIGTERM.
        // The method itself is an example and not useful.
        //app.MapGet("/Delay/{value}", async (int value) =>
        //{
        //    try
        //    {
        //        await Task.Delay(value, cancellation.Token);
        //    }
        //    catch(TaskCanceledException)
        //    {
        //    }
        //    return new Operation(value);
        //});

        return app;
    }

    public static async Task<WebApplication> InitializeServerAsync(
        this WebApplication app,
        IConfiguration configuration)
    {
        //await app.MigrateAsync<ApplicationDbContext>();
        await app.MigrateFactoryAsync<IApplicationDbContextFactory>();
        await app.SeedDataAsync();
        return app;
    }

    public static async Task SeedDataAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var applicationDbContextFactory = scope.ServiceProvider.GetRequiredService<IApplicationDbContextFactory>();

        await using var dbContext = await applicationDbContextFactory.CreateAsync();
        var tenants = await dbContext.Set<Tenant>().ToListAsync();

        var result = await host.SeedUsersAsync(scope, tenants);
        await host.SeedToDoItemsAsync(dbContext, tenants);
    }

    public static async Task<IdentityResult?> SeedUsersAsync(this IHost host, IServiceScope scope, System.Collections.Generic.List<Tenant> tenants)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (await userManager.Users.AnyAsync())
        {
            return null;
        }

        var adminUser = new ApplicationUser
        {
            UserName = "admin@asd.com",
            TenantId = tenants[0].Id,
            Email = "admin@asd.com",
            EmailConfirmed = true,
            PhoneNumber = "123",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            TenantUsers = tenants.Select(t => new TenantUser { TenantId = t.Id }).ToList()
        };
        return await userManager.CreateAsync(adminUser, "Password123!");
    }

    public static async Task SeedToDoItemsAsync(this IHost host, IDbContext dbContext, List<Tenant> tenants)
    {
        if(await dbContext.Set<ToDoItem>().AnyAsync())
        {
            return;
        }

        var toDoItems = new[]
        {
            new ToDoItem { Title = "ToDoItem1", TenantId = tenants[0].Id },
            new ToDoItem { Title = "ToDoItem2", TenantId = tenants[1].Id }
        };

        await dbContext.Set<ToDoItem>().AddRangeAsync(toDoItems);
        await dbContext.SaveChangesAsync();
    }
}
