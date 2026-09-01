using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Poc.Auth.Blazor.Options;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Poc.Auth.Blazor.Extensions;

public static partial class AuthenticationExtensions
{
    // add basic entra auth
    public static void AddEntraBasicAuthentications(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        if (configurationSection == null) throw new Exception();

        services.Configure<EntraIdOptions>(configurationSection);

        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configurationSection);
    }

    // add advanced entra auth
    public static void AddEntraAuthentications(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        if (configurationSection == null) throw new Exception();

        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configurationSection)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();
    }

    // add graph
    public static void AddGraphDownstreamApi(
        this IServiceCollection services,
        IConfigurationSection graphConfigurationSection)
    {
        services.Configure<MicrosoftGraphOptions>(graphConfigurationSection);

        var microsoftGraphOptions = graphConfigurationSection.Get<MicrosoftGraphOptions>();

        //services.AddScoped<GraphServiceClient, GraphServiceClient>(serviceProvider =>
        //{
        //    IAuthorizationHeaderProvider authorizationHeaderProvider = serviceProvider.GetRequiredService<IAuthorizationHeaderProvider>();

        //    return graphServiceClientFactory(new TokenAcquisitionAuthenticationProvider(
        //        authorizationHeaderProvider,
        //        new TokenAcquisitionAuthenticationProviderOption() { Scopes = initialScopes.ToArray() }));
        //        new TokenAcquisitionAuthenticationProviderOption() { AppOnly = true }));
        //});
    }

    // add auth and graph DownstreamApi
    public static void AddDownstreamApi(
        this IServiceCollection services,
        IConfigurationSection entraConfigurationSection,
        IConfigurationSection graphConfigurationSection)
    {
        var microsoftGraphOptions = graphConfigurationSection.Get<MicrosoftGraphOptions>();

        // This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
        // By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
        // 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
        // This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token.
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(entraConfigurationSection)
            .EnableTokenAcquisitionToCallDownstreamApi(microsoftGraphOptions.AppScopes)
            //.AddMicrosoftGraph(graphConfigurationSection)
            .AddInMemoryTokenCaches();
    }

    // add basic google auth
    public static void AddGoogleBasicAuthentications(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddGoogle(configurationSection.GetGoogleOptions);
        //.AddGoogle(options => configurationSection.Bind(options));
    }

    private static void GetGoogleOptions(this IConfigurationSection configurationSection, Microsoft.AspNetCore.Authentication.Google.GoogleOptions options)
    {
        var googleOptions = configurationSection.Get<GoogleOptions>() ?? throw new Exception();

        options.ClientId = googleOptions.ClientId;
        options.ClientSecret = googleOptions.ClientSecret;
        //options.ClaimActions.MapJsonKey("urn:google:profile", "link");
        //options.ClaimActions.MapJsonKey("urn:google:image", "picture");
    }
}