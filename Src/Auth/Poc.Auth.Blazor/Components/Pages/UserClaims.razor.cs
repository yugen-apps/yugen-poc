using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages;

public partial class UserClaims
{
    private IEnumerable<Claim> claims = [];
    private ClaimsPrincipal? User;
    private string? GivenName;
    private string? Surname;
    private string? Avatar;

    [CascadingParameter]
    private Task<AuthenticationState>? AuthState { get; set; }

    // [Inject]
    // private AuthenticationStateProvider? AuthenticationStateProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (AuthState == null)
        {
            return;
        }

        // Gets an AuthenticationState that describes the current user.
        // AuthenticationState authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var authState = await AuthState;

        User = authState.User;

        claims = authState.User.Claims;

        // Try to get the GivenName
        var givenName = User.FindFirst(ClaimTypes.GivenName);
        GivenName = givenName != null ? givenName.Value : User.Identity?.Name ?? "Unknown";

        var surname = User.FindFirst(ClaimTypes.Surname);
        Surname = surname != null ? surname.Value : User.Identity?.Name ?? "Unknown";

        var avatar = User.FindFirst("urn:google:image");
        Avatar = avatar != null ? avatar.Value : "";


        // Checks if the user has been authenticated.
        //if (User.Identity != null &&
        //    User.Identity.IsAuthenticated)
        //{
        //    var userIdentity = User.Identity;

        //    // Sets the claims value in _claims variable.
        //    // The claims mentioned in printClaims variable are selected only.
        //    string[] printClaims = { "name", "preferred_username", "tid", "oid" };
        //    var claims = User.Claims.Where(x => printClaims.Contains(x.Type));

        //    var userIdentityString = JsonSerializer.Serialize(userIdentity, new JsonSerializerOptions { WriteIndented = true });
        //}
    }
}
