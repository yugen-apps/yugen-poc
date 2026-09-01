using Poc.Ef.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.Ef.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<IEmailService, EmailService>();
    }
}