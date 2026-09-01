using MudBlazor;
using System.Collections.Generic;
using Yugen.Common.Blazor.Navigation;

namespace Poc.Identity.Blazor.Components.Layout;

public static class MenuConstants
{
    public static List<MenuItem> Items =>
    [
        Home,
        Tenants,
        ToDo,
        Info
    ];

    public static MenuItem Home => new()
    {
        Href = RouteConstants.Home,
        Icon = Icons.Material.Filled.Home,
        Title = "Home"
    };

    public static MenuItem Info => new()
    {
        Href = RouteConstants.Info,
        Icon = Icons.Material.Filled.Info,
        Policy = PermissionsConstants.Info,
        Title = "Info"
    };

    public static MenuItem Tenants => new()
    {
        Href = RouteConstants.Tenants,
        Icon = Icons.Material.Filled.AccountBalance,
        Policy = PermissionsConstants.Tenants,
        Title = "Tenants"
    };

    public static MenuItem ToDo => new()
    {
        Icon = Icons.Material.Filled.ListAlt,
        Policy = PermissionsConstants.ToDo.Base,
        Title = "ToDo",
        MenuSubItems = [
            new MenuItem
            {
                Href = RouteConstants.ToDo.All,
                Icon = Icons.Material.Filled.Checklist,
                Policy = PermissionsConstants.ToDo.All,
                Title = "All"
            },
            new MenuItem
            {
                Href = RouteConstants.ToDo.Tenants,
                Icon = Icons.Material.Filled.Checklist,
                Policy = PermissionsConstants.ToDo.Tenants,
                Title = "Tenants"
            }
       ]
    };
}
