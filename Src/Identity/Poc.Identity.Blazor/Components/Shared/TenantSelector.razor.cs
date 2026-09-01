using Microsoft.AspNetCore.Components;
using MudBlazor;
using Poc.Identity.Blazor.Components.Layout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;
using Yugen.Common.Blazor.Account.Features.Users.Services;

namespace Poc.Identity.Blazor.Components.Shared;

public partial class TenantSelector
{
    private readonly NavigationManager _navigationManager;
    private readonly ISnackbar _snackbar;
    private readonly ITenantService _tenantService;
    private readonly IUserService _userService;

    public TenantSelector(
        NavigationManager navigationManager,
        ISnackbar snackbar,
        ITenantService tenantService,
        IUserService userService)
    {
        _navigationManager = navigationManager;
        _snackbar = snackbar;
        _tenantService = tenantService;
        _userService = userService;
    }

    private UserDto UserProfile => _userService.UserProfileState;
    private List<TenantDto>? AvailableTenants => UserProfile.AvailableTenants;
    private bool _isSwitching;

    protected override async Task OnInitializedAsync()
    {
        _userService.UserChanged += OnUserProfileChanged;
    }

    public void Dispose()
    {
        _userService.UserChanged -= OnUserProfileChanged;
    }

    private void OnUserProfileChanged(object? sender, UserDto userDto)
    {
        InvokeAsync(StateHasChanged);
    }

    private async Task SwitchToTenant(Guid tenantId)
    {
        if (_isSwitching ||
            UserProfile?.Id == null)
        {
            return;
        }

        _isSwitching = true;
        try
        {
            var result = await _tenantService.SwitchToTenantAsync(UserProfile.Id, tenantId);
            if (result.IsSuccess)
            {
                _snackbar.Add("Tenant switched successfully", Severity.Success);

                // Refresh user state
                //await _userService.RefreshAsync();

                // Navigate to home page
                _navigationManager.NavigateTo(RouteConstants.Home, true);
            }
            else
            {
                _snackbar.Add(result.Error?.Message ?? "Failed to switch tenant", Severity.Error);
            }
        }
        finally
        {
            _isSwitching = false;
        }
    }
}
