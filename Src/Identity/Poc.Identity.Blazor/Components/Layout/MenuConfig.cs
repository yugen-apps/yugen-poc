using System.Collections.Generic;
using Yugen.Common.Blazor.Navigation;

namespace Poc.Identity.Blazor.Components.Layout;

public class MenuConfig
{
    public IEnumerable<MenuItem>? Items { get; set; }
}
