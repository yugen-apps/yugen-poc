using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Auth.Services;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;
using Yugen.Common.Blazor.Account.Features.Users.Services;

namespace Poc.Identity.Blazor.Components.Layout;

public partial class MainLayout
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ILogger<MainLayout> _logger;

    public MainLayout(
        IAuthService authService,
        IUserService userService,
        ILogger<MainLayout> logger)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    private bool _drawerOpen = true;

    protected override async Task OnInitializedAsync()
    {
        _userService.UserChanged += OnUserProfileChanged;

        var user = await _authService.GetCurrentUserAsync();
        var userId = user?.Id;
        if (user?.IsAuthenticated == false ||
            userId == null)
        {
            return;
        }

        await _userService.EnsureInitializedAsync(userId);
    }

    public void Dispose()
    {
        _userService.UserChanged -= OnUserProfileChanged;
    }

    private void OnUserProfileChanged(object? sender, UserDto userDto)
    {
        InvokeAsync(StateHasChanged);
    }
}
