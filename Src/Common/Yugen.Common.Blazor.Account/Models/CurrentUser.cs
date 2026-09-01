using System.Security.Claims;

namespace Yugen.Common.Blazor.Account.Models;

public class CurrentUser
{
    public CurrentUser(ClaimsPrincipal user)
    {
        User = user;
        IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;

        if (IsAuthenticated)
        {
            UserName = User?.Identity?.Name;
            GivenName = User?.FindFirst(ClaimTypes.GivenName)?.Value;
            Surname = User?.FindFirst(ClaimTypes.Surname)?.Value;
            Avatar = User?.FindFirst(GoogleClaimTypes.Picture)?.Value;

            var currentUserIdentifier = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(currentUserIdentifier, out var currentUserId))
            {
                Id = currentUserId;
            }
        }
    }

    public ClaimsPrincipal User { get; }

    public bool IsAuthenticated { get; }

    public string UserName { get; }

    public string GivenName { get; }

    public string Surname { get; }

    public string Avatar { get; }

    public int Id { get; set; }
}
