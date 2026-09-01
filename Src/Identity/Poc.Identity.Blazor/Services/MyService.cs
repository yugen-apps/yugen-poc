using Microsoft.Extensions.Options;
using Poc.Identity.Blazor.Options;

namespace Poc.Identity.Blazor.Services;

public class MyService
{
    private readonly EntraIdOptions _options;
    public MyService(IOptions<EntraIdOptions> options)
    {
        _options = options.Value;
    }
}
