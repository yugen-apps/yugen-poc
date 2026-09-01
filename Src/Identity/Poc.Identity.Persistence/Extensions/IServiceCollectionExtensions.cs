using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Identity.Persistence.Contexts;
using System.Threading.Tasks;
using Yugen.Common.Data.Extensions;

namespace Poc.Identity.Persistence.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration);
        services.AddPersistenceFactory<ApplicationDbContext, ApplicationDbContextFactory>(configuration);
        services.AddScoped<ApplicationDbContextInitializer>();
        return services;
    }

    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitializeAsync();

        var env = host.Services.GetRequiredService<IHostEnvironment>();
        if (env.IsDevelopment())
        {
            await initializer.SeedDataAsync().ConfigureAwait(false);
        }
    }
}