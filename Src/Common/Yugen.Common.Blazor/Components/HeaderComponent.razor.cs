using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;

namespace Yugen.Common.Blazor.Components;

public partial class HeaderComponent
{
    [Parameter]
    public string PageTitle { get; set; }

    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public string Breadcrumb { get; set; }

    private List<BreadcrumbItem> _items = [];

    protected override void OnInitialized()
    {
        _items =
        [
            new("Home", href: "/"),
            new(Breadcrumb, href: null),
            //new("Link 1", href: "#"),
            //new("Link 2", href: null, disabled: true)
        ];

        base.OnInitialized();
    }
}