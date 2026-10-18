using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Data.Commands;
using Yugen.Common.Data.Helpers;

namespace Yugen.Common.Data.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : DbContext
    {
        services.AddDbContext<T>(configuration, serviceLifetime);
        services.AddScoped(typeof(IDbContext), typeof(T));
        return services;
    }

    public static IServiceCollection AddPersistenceFactory<T, T2>(
        this IServiceCollection services,
        IConfiguration configuration)
            where T : DbContext
            where T2 : IApplicationDbContextFactory
    {
        services.AddDbContextFactory<T>(configuration);
        services.AddScoped(typeof(IDbContext), typeof(T));
        services.AddScoped(typeof(IApplicationDbContextFactory), typeof(T2));
        return services;
    }

    private static IServiceCollection AddDbContext<T>(
        this IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime serviceLifetime) where T : DbContext
    {
        var msSqlConnectionString = configuration.GetConnectionString("MsSql");
        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");

        if (!string.IsNullOrWhiteSpace(msSqlConnectionString))
        {
            services.AddDbContext<T>(options =>
               options.UseSqlServer(msSqlConnectionString,
                   builder => builder.MigrationsAssembly(typeof(T).Assembly.FullName)), serviceLifetime);
        }
        else if (!string.IsNullOrWhiteSpace(sqliteConnectionString))
        {
            services.AddDbContext<T>(options =>
               options.UseSqlite(sqliteConnectionString,
                   builder => builder.MigrationsAssembly(typeof(T).Assembly.FullName)), serviceLifetime);
        }
        else
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var connection = SqliteInMemoryHelper.Initialize();

            // These options will be used by the dbContext instances in this test suite, including the connection opened above.
            services.AddDbContext<T>(options =>
               options.UseSqlite(connection), ServiceLifetime.Singleton);
        }
        return services;
    }

    private static IServiceCollection AddDbContextFactory<T>(
        this IServiceCollection services,
        IConfiguration configuration) where T : DbContext
    {
        var msSqlConnectionString = configuration.GetConnectionString("MsSql");
        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");

        if (!string.IsNullOrWhiteSpace(msSqlConnectionString))
        {
            services.AddDbContextFactory<T>(options =>
                options.UseSqlServer(msSqlConnectionString,
                    builder => builder.MigrationsAssembly(typeof(T).Assembly.FullName)));
        }
        else if (!string.IsNullOrWhiteSpace(sqliteConnectionString))
        {
            services.AddDbContextFactory<T>(options =>
                options.UseSqlite(sqliteConnectionString,
                    builder => builder.MigrationsAssembly(typeof(T).Assembly.FullName)));
        }
        else
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            var connection = SqliteInMemoryHelper.Initialize();

            // These options will be used by the dbContext instances in this test suite, including the connection opened above.
            services.AddDbContextFactory<T>(options =>
                options.UseSqlite(connection));
        }
        return services;
    }

    public static async Task EnsureCreatedAsync<T>(this IHost host) where T : IDbContext
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        await dbContext.EnsureCreatedAsync();
    }

    public static async Task MigrateAsync<T>(this IHost host) where T : IDbContext
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        await dbContext.MigrateAsync();
    }

    public static async Task EnsureCreatedFactoryAsync<T>(this IHost host) where T : IApplicationDbContextFactory
    {
        using var scope = host.Services.CreateScope();
        var applicationDbContextFactory = scope.ServiceProvider.GetRequiredService<T>();
        await using var dbContext = await applicationDbContextFactory.CreateAsync();
        await dbContext.EnsureCreatedAsync();
    }

    public static async Task MigrateFactoryAsync<T>(this IHost host) where T : IApplicationDbContextFactory
    {
        using var scope = host.Services.CreateScope();
        var applicationDbContextFactory = scope.ServiceProvider.GetRequiredService<T>();
        await using var dbContext = await applicationDbContextFactory.CreateAsync();
        await dbContext.MigrateAsync();
    }

    //public static async Task InitializeAsync<T>(this IHost host) where T : IDbContext
    //{
    //    using var scope = host.Services.CreateScope();
    //    var applicationDbContext = scope.ServiceProvider.GetRequiredService<T>();
    //    await applicationDbContext.SeedAsync();
    //}

    public static void AddCommandHandlers(
        this IServiceCollection services,
        Assembly assembly)
    {
        Type[] compareTypes = [
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>)
            ];

        foreach (var compareType in compareTypes)
        {
            var typeInfoList = assembly.DefinedTypes.Where(x =>
               x.IsClass &&
               !x.IsAbstract &&
               x != compareType &&
               x.GetInterfaces()
                 .Any(i => i.IsGenericType &&
                           i.GetGenericTypeDefinition() == compareType));

            foreach (var type in typeInfoList)
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    services.AddScoped(implementedInterface, type);
                }
            }
        }
    }
}
