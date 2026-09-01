using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poc.Auth.Blazor.Services.Graph;
using Poc.Common;
using Poc.Common.Web;
using System;

namespace Poc.Auth.Blazor.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<GraphService>();
        services.AddScoped<SystemInfoService>();
		services.AddScoped<WebHostInfoService>();
	}

    public static void InitializeServices(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AzureIdentityEventTracker>>();
        using AzureIdentityEventTracker tracker = new(logger);
    }

    //IConfiguration
    //var appConfig = configuration.Get<AppConfig>();
    //IConfigurationSection section = builder.Configuration.GetSection("DownstreamApi");        
    //string? test = configuration.GetValue<string>("Test");

    //IOptions
    //services.Configure<EntraIdOptions>(configuration.GetSection("EntraId"));
}
