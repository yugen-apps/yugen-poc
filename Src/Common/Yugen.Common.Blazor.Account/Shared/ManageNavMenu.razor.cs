using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Common.Blazor.Account.Domain.Entities;
using Yugen.Common.Blazor.Account.Features.Navigation;
using Yugen.Common.Blazor.Navigation;

namespace Yugen.Common.Blazor.Account.Shared;

public partial class ManageNavMenu
{
    private readonly IEnumerable<MenuItem> MenuItems;
    private readonly NavigationManager _navigationManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private string? currentUrl;
    private bool hasExternalLogins;

    public ManageNavMenu(
        NavigationManager navigationManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<AccountMenuConfig> menuOptions)
    {
        _navigationManager = navigationManager;
        _signInManager = signInManager;
        MenuItems = menuOptions.Value?.Items ?? [];
    }

    protected override async Task OnInitializedAsync()
    {
        hasExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).Any();
        currentUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
        _navigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = _navigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}
