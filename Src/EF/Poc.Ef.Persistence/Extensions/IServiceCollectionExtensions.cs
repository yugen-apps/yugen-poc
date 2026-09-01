using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Common.Data;
using Poc.Ef.Application.Contracts.Repositories;
using Poc.Ef.Persistence.Contexts;
using Poc.Ef.Persistence.Helpers;
using Poc.Ef.Persistence.Repositories;
using System;

namespace Poc.Ef.Persistence.Extensions;

/// <summary>
/// dotnet ef migrations add v1 --project .\Poc.Ef.Persistence\ --startup-project .\Poc.Ef.Api\
/// dotnet ef migrations add v1 --project .\Poc.Ef.Persistence\ --startup-project .\Poc.Ef.Aspnet\
/// dotnet ef migrations add v1 --project .\Poc.Ef.Persistence\ --startup-project .\Poc.Ef.WorkerService\
/// </summary>
public static class IServiceCollectionExtensions
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var msSqlConnectionString = configuration.GetConnectionString("MsSql");
        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");

        if (!string.IsNullOrWhiteSpace(msSqlConnectionString))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(msSqlConnectionString,
                   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        else if (!string.IsNullOrWhiteSpace(sqliteConnectionString))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(sqliteConnectionString,
                   builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        else
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var connection = SqliteInMemoryHelper.Initialize();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(connection), ServiceLifetime.Singleton);
        }
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IBaseRepository, BaseRepository>(sp => new BaseRepository(sp.GetRequiredService<ApplicationDbContext>()))
            .AddScoped<IUnitOfWork, UnitOfWork>(sp => new UnitOfWork(sp.GetRequiredService<ApplicationDbContext>()))
            .AddScoped<ICategoryRepository, CategoryRepository>();
    }

    public static void EnsureCreated(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        applicationDbContext.Database.EnsureCreated();
    }
}
