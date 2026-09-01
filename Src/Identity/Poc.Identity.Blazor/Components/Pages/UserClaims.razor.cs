using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poc.Identity.Blazor.Components.Pages;

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
	}
}
