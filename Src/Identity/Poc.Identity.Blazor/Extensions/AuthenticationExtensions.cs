using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using System;

namespace Poc.Identity.Blazor.Extensions;

public static partial class AuthenticationExtensions
{
    // add basic entra auth
    public static void AddEntraBasicAuthentications(
        this IServiceCollection services,
        IConfiguration configuration,
        string key)
    {
        var entraSection = configuration.GetSection(key) ?? throw new Exception();

        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(entraSection);
    }

    // add basic google auth
    public static void AddGoogleBasicAuthentications(
        this IServiceCollection services,
        IConfiguration configuration,
        string key)
    {
        var googleOptions = configuration.GetSection(key).Get<Options.GoogleOptions>() ?? throw new Exception();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie();

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = googleOptions.ClientId;
                options.ClientSecret = googleOptions.ClientSecret;
                //options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                //options.ClaimActions.MapJsonKey("urn:google:image", "picture");
            });
    }

    public static void AddMoreAuthentications(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //IConfiguration
        //var appConfig = configuration.Get<AppConfig>();
        //IConfigurationSection section = builder.Configuration.GetSection("DownstreamApi");        
        //string? test = configuration.GetValue<string>("Test");

        //IOptions
        //services.Configure<EntraIdOptions>(configuration.GetSection("EntraId"));


        // add advanced entra auth
        //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApp(configuration.GetSection("EntraId"))
        //    .EnableTokenAcquisitionToCallDownstreamApi()
        //    .AddInMemoryTokenCaches();

        // add auth and DownstreamApi
        //string[] initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ') ?? Array.Empty<string>();
        //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApp(configuration.GetSection("EntraId"))
        //    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        //    .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
        //    .AddInMemoryTokenCaches();

        // add auth and graph
        //var initialScopes = builder.Configuration.GetValue<IEnumerable<string>>("Graph:Scopes");


        // TODO: check if still needed
        // This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
        // By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
        // 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
        // This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token.
        // JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApp(configuration.GetSection("EntraId"))
        //    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        //    .AddMicrosoftGraph(builder.Configuration.GetSection("Graph"))
        //    .AddInMemoryTokenCaches();

    }
}
