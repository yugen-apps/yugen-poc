using Microsoft.Extensions.DependencyInjection;
using Yugen.Common.Data.Extensions;

namespace Poc.Ef.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddCommandHandlers(typeof(IServiceCollectionExtensions).Assembly);
        return services;
    }
}