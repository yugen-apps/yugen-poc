using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Poc.Identity.Blazor.Models;
using System;

namespace Poc.Identity.Blazor.Extensions;

public static class EndpointsAuthenticationEntraExtensions
{
    public static void MapAuthenticationEntraEndpoints(this WebApplication app)
    {
        var mapGroup = app.MapGroup("/authentication");
        mapGroup.MapGet("/login", Login).AllowAnonymous();
        mapGroup.MapPost("/logout", Logout).AllowAnonymous();
    }

    private static IResult Login([FromQuery] string? returnUrl)
    {
        return TypedResults.Challenge(GetAuthProperties(returnUrl));
    }

    // Sign out with both the Cookie and OIDC authentication schemes. Users who have not signed out with the OIDC scheme will
    // automatically get signed back in as the same user the next time they visit a page that requires authentication
    // with no opportunity to choose another account.
    private static IResult Logout([FromForm] string? returnUrl)
    {
        return TypedResults.SignOut(GetAuthProperties(returnUrl),
            [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]);
    }

    // Prevent open redirects. Non-empty returnUrls are absolute URIs provided by NavigationManager.Uri.
    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        returnUrl = string.IsNullOrWhiteSpace(returnUrl)
            ? null
            : $"{AppConstants.BaseUrl}/{returnUrl}";

        return new AuthenticationProperties()
        {
            RedirectUri = returnUrl switch
            {
                string => new Uri(returnUrl, UriKind.Absolute).PathAndQuery,
                null => "/",
            }
        };
    }
}
