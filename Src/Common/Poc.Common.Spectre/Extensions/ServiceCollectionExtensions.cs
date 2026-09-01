using Microsoft.Extensions.DependencyInjection;

namespace Poc.Common.Spectre.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpectre(
        this IServiceCollection services)
    {
        return services
            .AddSingleton<TypeResolver>();
    }
}
