using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Identity.Blazor.Services;
using System;

namespace Poc.Identity.Blazor.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<MyService>();
    }

    public static void InitializeServices(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var myService = scope.ServiceProvider.GetRequiredService<MyService>();
    }
}
