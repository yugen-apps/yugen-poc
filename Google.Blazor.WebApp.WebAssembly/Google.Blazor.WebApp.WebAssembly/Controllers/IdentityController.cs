using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Google.Blazor.WebApp.WebAssembly.Controllers
{
    [ApiController]
    [Route("Identity")]
    [AllowAnonymous]
    public class IdentityController : Controller
    {
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(ILogger<IdentityController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Login")]
        public IActionResult OnGetLoginAsync(string returnUrl = null)
        {
            return new ChallengeResult("Google", new()
            {
                RedirectUri = "https://localhost:7287/Identity/LoginCallback"
            });
        }

        [HttpGet("LoginCallback")]
        public async Task<IActionResult> OnGetLoginCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            // Get the information about the user from the external login provider
            var user = User.Identities.FirstOrDefault();
            if (user?.IsAuthenticated ?? false)
            {
                var email = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))?.Value;
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new(user),
                    new()
                    {
                        IsPersistent = true,
                        RedirectUri = Request.Host.Value
                    });
            }
            else
            {
            }

            return LocalRedirect("/");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> OnGetLogoutAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            try
            {
                await HttpContext
                    .SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to logout");
                var error = ex.Message;
            }

            return LocalRedirect(returnUrl);
        }
    }
}
