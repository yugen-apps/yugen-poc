using Microsoft.AspNetCore.Components;
using MudBlazor;
using Poc.Identity.Blazor.Services.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Features.Tenants.DTOs;
using Yugen.Common.Blazor.Account.Features.Tenants.Services;
using Yugen.Common.Blazor.Account.Features.Users.DTOs;

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

    private UserDto _userProfile => _userService.UserProfileState;
    private List<TenantDto>? _availableTenants => _userProfile.AvailableTenants;
    private bool _isSwitching;

    protected override async Task OnInitializedAsync()
    {
        _userService.UserChanged += OnUserProfileChanged;

        // TODO: move to app layout
        await _userService.InitializedAsync();
    }

    public void Dispose()
    {
        _userService.UserChanged -= OnUserProfileChanged;
    }

    private void OnUserProfileChanged(object? sender, EventArgs eventArgs)
    {
        InvokeAsync(StateHasChanged);
    }

    private async Task SwitchToTenant(Guid tenantId)
    {
        if (_isSwitching ||
            _userProfile?.Id == null)
        {
            return;
        }

        _isSwitching = true;
        try
        {
            var result = await _tenantService.SwitchToTenantAsync(_userProfile.Id, tenantId);
            if (result.IsSuccess)
            {
                _snackbar.Add("Tenant switched successfully", Severity.Success);

                // Refresh user state
                //await _userService.RefreshAsync();

                // Navigate to home page
                _navigationManager.NavigateTo("/", true);
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
