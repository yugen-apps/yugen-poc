using Ef.Poc.Application.Contracts.Services;
using Ef.Poc.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ef.Poc.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<ICategoryService, CategoryService>();
    }
}