using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Identity.Blazor.Components;
using Poc.Identity.Blazor.Components.Layout;
using Poc.Identity.Persistence.Contexts;
using Yugen.Common.Blazor.Account;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Extensions;
using Yugen.Common.Blazor.Account.Features.Auth;
using Yugen.Common.Blazor.Account.Features.Auth.Services;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Features.Users.Services;
using Yugen.Common.Info;
using Yugen.Common.Web.Info;

namespace Poc.Identity.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // services.AddHealthChecks();

        // Uncomment if using System.Text.Json source generation
        // services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

        services.Configure<MenuConfig>(options => options.Items = MenuConstants.Items);

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<SystemInfoService>();
        services.AddScoped<WebHostInfoService>();
        return services;
    }

    public static IServiceCollection AddIdentity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAccountMenu(configuration);
        services.AddCascadingAuthenticationState();
        services.AddAuthorization(options => options.AddPolicy());
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
        .AddRoles<ApplicationRole>()
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
            app.UseExceptionHandler(RouteConstants.Error);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseStatusCodePagesWithReExecute(RouteConstants.NotFound, createScopeForStatusCodePages: true);

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
}
