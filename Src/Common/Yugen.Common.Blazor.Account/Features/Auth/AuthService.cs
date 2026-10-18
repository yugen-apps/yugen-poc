using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Models;

namespace Yugen.Common.Blazor.Account.Features.Auth;

public class AuthService : IAuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<CurrentUser> GetCurrentUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;
        return new CurrentUser(user);
    }
}
