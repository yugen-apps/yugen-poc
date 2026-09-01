

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yugen.Common.Identity.Blazor.Features.Auth;
using Yugen.Common.Identity.Blazor.Features.Auth.Services;
using Yugen.Common.Identity.Blazor.Features.Navigation;
using Yugen.Common.Identity.Core.Features.Tenants.Services;
using Yugen.Common.Identity.Core.Features.Users.Services;
using Yugen.Common.Identity.Domain.Entities;

namespace Yugen.Common.Identity.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityBlazor<T>(
        this IServiceCollection services,
        IConfiguration configuration) where T : DbContext
    {
        services.Configure<AccountMenuConfig>(options => options.Items = AccountMenuConstants.Items);
        services.AddCascadingAuthenticationState();
        //services.AddAuthorization(options => options.AddPolicy());
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
        .AddEntityFrameworkStores<T>()
        .AddSignInManager()
        .AddDefaultTokenProviders();
        return services;
    }
}
