using Microsoft.AspNetCore.Components;
using Yugen.Common.Blazor.Navigation;

namespace Yugen.Common.Blazor.Components;

public partial class MenuItemComponent
{
    [Parameter]
    public MenuItem MenuItem { get; set; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}