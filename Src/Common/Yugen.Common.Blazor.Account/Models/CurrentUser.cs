using System;
using System.Security.Claims;

namespace Yugen.Common.Blazor.Account.Models;

public class CurrentUser
{
    public CurrentUser(ClaimsPrincipal user)
    {
        IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;
        if (!IsAuthenticated)
        {
            return;
        }

        UserName = user?.Identity?.Name;
        Email = user?.FindFirst(ClaimTypes.Email)?.Value;
        GivenName = user?.FindFirst(ClaimTypes.GivenName)?.Value;
        Surname = user?.FindFirst(ClaimTypes.Surname)?.Value;
        Avatar = user?.FindFirst(GoogleClaimTypes.Picture)?.Value;

        var currentUserIdentifier = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(currentUserIdentifier, out var currentUserId))
        {
            Id = currentUserId;
        }
    }

    public bool IsAuthenticated { get; }

    public string? UserName { get; }

    public string? Email { get; }

    public string? GivenName { get; }

    public string? Surname { get; }

    public string? Avatar { get; }

    public Guid? Id { get; }
}