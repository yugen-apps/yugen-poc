using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Common;
using Poc.Common.Web;

namespace Poc.Auth.Blazor.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<SystemInfoService>();
        services.AddScoped<WebHostInfoService>();
	}
}
