using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Yugen.Common.Blazor.Navigation;

namespace Poc.Identity.Blazor.Components.Layout;

public partial class NavMenu
{
    private readonly IEnumerable<MenuItem> MenuItems;

    public NavMenu(IOptions<MenuConfig> menuOptions)
    {
        MenuItems = menuOptions.Value?.Items ?? [];
    }
}
