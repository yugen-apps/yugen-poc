using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Poc.Identity.Blazor.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poc.Identity.Blazor.Extensions;

public static class EndpointsAuthenticationGoogleExtensions
{
    public static void MapAuthenticationGoogleEndpoints(this WebApplication app)
    {
        var mapGroup = app.MapGroup("/identity");
        mapGroup.MapGet("/login", Login).AllowAnonymous();
        mapGroup.MapGet("/LoginCallback", LoginCallbackAsync).AllowAnonymous();
        mapGroup.MapPost("/logout", Logout).AllowAnonymous();
    }

    private static IResult Login([FromQuery] string? returnUrl)
    {
        return TypedResults.Challenge(new AuthenticationProperties
        {
            RedirectUri = $"{AppConstants.BaseUrl}/Identity/LoginCallback"
        }, ["Google"]);
    }

    private static async Task<IResult> LoginCallbackAsync(
        [FromQuery] string? returnUrl,
        [FromQuery] string? remoteError,
        HttpContext httpContext)
    {
        // Get the information about the user from the external login provider
        var user = httpContext.User.Identities.FirstOrDefault();
        if (user?.IsAuthenticated ?? false)
        {
            var email = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(user),
                new AuthenticationProperties()
                {
                    IsPersistent = true, 
                });
        }

        return TypedResults.LocalRedirect("/");
    }

    // Sign out with both the Cookie and OIDC authentication schemes. Users who have not signed out with the OIDC scheme will
    // automatically get signed back in as the same user the next time they visit a page that requires authentication
    // with no opportunity to choose another account.
    private static async Task<IResult> Logout(
        [FromForm] string? returnUrl,
        HttpContext httpContext)
    {
        returnUrl ??= "/";

        try
        {
            await httpContext
                .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to logout");
            var error = ex.Message;
        }

        return TypedResults.LocalRedirect(returnUrl);
    }
}



//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace Google.Blazor.WebApp.WebAssembly.Controllers;

//[ApiController]
//[Route("Identity")]
//[AllowAnonymous]
//public class IdentityController : Controller
//{
//    private readonly ILogger<IdentityController> _logger;

//    public IdentityController(ILogger<IdentityController> logger)
//    {
//        _logger = logger;
//    }

//    [HttpGet("Login")]
//    public IActionResult OnGetLoginAsync(string returnUrl = null)
//    {
//        return new ChallengeResult("Google", new()
//        {
//            RedirectUri = $"{AppConstants.BaseUrl}/Identity/LoginCallback"
//        });
//    }

//    [HttpGet("LoginCallback")]
//    public async Task<IActionResult> OnGetLoginCallbackAsync(string returnUrl = null, string remoteError = null)
//    {
//        // Get the information about the user from the external login provider
//        var user = User.Identities.FirstOrDefault();
//        if (user?.IsAuthenticated ?? false)
//        {
//            var email = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
//            await HttpContext.SignInAsync(
//                CookieAuthenticationDefaults.AuthenticationScheme,
//                new(user),
//                new()
//                {
//                    IsPersistent = true,
//                    RedirectUri = Request.Host.Value
//                });
//        }
//        else
//        {
//        }

//        return LocalRedirect("/");
//    }

//    [HttpGet("Logout")]
//    public async Task<IActionResult> OnGetLogoutAsync(string returnUrl = null)
//    {
//        returnUrl ??= Url.Content("~/");

//        try
//        {
//            await HttpContext
//                .SignOutAsync(
//                    CookieAuthenticationDefaults.AuthenticationScheme);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Failed to logout");
//            var error = ex.Message;
//        }

//        return LocalRedirect(returnUrl);
//    }
//}
