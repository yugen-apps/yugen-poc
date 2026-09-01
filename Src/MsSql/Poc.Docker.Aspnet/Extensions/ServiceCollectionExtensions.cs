using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Common;
using Poc.Common.Web;

namespace Poc.Docker.Aspnet.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<SystemInfoService>();
        services.AddScoped<WebHostInfoService>();
    }
}
