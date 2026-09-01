using MudBlazor;
using System.Collections.Generic;

namespace Yugen.Common.Blazor.Navigation;

public class MenuItem
{
    public string Icon { get; set; } = Icons.Material.Filled.Dashboard;

    public bool IsDisabled { get; set; }

    public bool IsParent => MenuSubItems.Count > 0;

    public IList<MenuItem> MenuSubItems { get; set; } = [];

    public string? Href { get; set; }

    public string? Policy { get; set; }

    public PageStatus PageStatus { get; set; } = PageStatus.Completed;

    public string[]? Roles { get; set; }

    public string Subtitle { get; set; } = string.Empty;

    public string? Target { get; set; }

    public string Title { get; set; } = string.Empty;
}
