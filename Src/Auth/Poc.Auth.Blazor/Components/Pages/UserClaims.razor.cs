using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages;

public partial class UserClaims
{
    private IEnumerable<Claim> _claims = [];
    private ClaimsPrincipal? _user;
    private string? _givenName;
    private string? _surname;
    private string? _avatar;

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

        _user = authState.User;

        _claims = authState.User.Claims;

        // Try to get the GivenName
        var givenName = _user.FindFirst(ClaimTypes.GivenName);
        _givenName = givenName != null ? givenName.Value : _user.Identity?.Name ?? "Unknown";

        var surname = _user.FindFirst(ClaimTypes.Surname);
        _surname = surname != null ? surname.Value : _user.Identity?.Name ?? "Unknown";

        var avatar = _user.FindFirst("urn:google:image");
        _avatar = avatar != null ? avatar.Value : "";


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
