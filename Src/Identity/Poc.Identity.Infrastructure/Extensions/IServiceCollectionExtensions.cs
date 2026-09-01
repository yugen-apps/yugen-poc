using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Identity.Infrastructure.Services;

namespace Poc.Identity.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddTransient<IEmailService, EmailService>();
    }
}