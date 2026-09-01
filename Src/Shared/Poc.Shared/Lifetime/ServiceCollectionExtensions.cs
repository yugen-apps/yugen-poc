using Microsoft.Extensions.DependencyInjection;

namespace Poc.Shared.Lifetime;

public static class ServiceCollectionExtensions
{
    public static void AddLifetime(this IServiceCollection services)
    {
        services.AddTransient<IOperationTransient, Operation>();
        services.AddScoped<IOperationScoped, Operation>();
        services.AddSingleton<IOperationSingleton, Operation>();

        services.AddTransient<OperationService>();
        //services.AddScoped<OperationService>();
        //services.AddSingleton<OperationService>();
    }
}
