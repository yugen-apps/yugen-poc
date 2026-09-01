using Poc.Ef.Application.Contracts.Services;
using Poc.Ef.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.Ef.Application.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<ICategoryService, CategoryService>();
    }
}