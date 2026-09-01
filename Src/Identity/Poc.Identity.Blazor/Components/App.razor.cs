using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;

namespace Poc.Identity.Blazor.Components;

public partial class App
{
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    private IComponentRenderMode? PageRenderMode => HttpContext?.AcceptsInteractiveRouting() ?? false
        ? new InteractiveServerRenderMode(false)
        : null;
}
