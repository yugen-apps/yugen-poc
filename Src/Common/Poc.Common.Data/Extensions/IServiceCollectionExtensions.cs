using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Common.Data.Helpers;
using System;

namespace Poc.Common.Data.Extensions;

public static class IServiceCollectionExtensions
{
	public static void AddPersistence<T>(
		this IServiceCollection services,
		IConfiguration configuration,
		ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : DbContext
	{
		services.AddDbContext<T>(configuration, serviceLifetime);
		services.AddScoped(typeof(IDbContext), typeof(T));

		// services.AddRepositories();
	}

	private static void AddDbContext<T>(
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

			// These options will be used by the context instances in this test suite, including the connection opened above.
			services.AddDbContext<T>(options =>
			   options.UseSqlite(connection), ServiceLifetime.Singleton);
		}
	}

	// private static void AddRepositories(this IServiceCollection services)
	// {
	//     services
	//         .AddScoped<IBaseRepository, BaseRepository>(sp => new BaseRepository(sp.GetRequiredService<ApplicationDbContext>()))
	//         .AddScoped<IUnitOfWork, UnitOfWork>(sp => new UnitOfWork(sp.GetRequiredService<ApplicationDbContext>()))
	//         .AddScoped<ICategoryRepository, CategoryRepository>();
	// }

	public static void EnsureCreated<T>(this IServiceProvider serviceProvider) where T : DbContext
	{
		using var scope = serviceProvider.CreateScope();
		var applicationDbContext = scope.ServiceProvider.GetRequiredService<T>();
		applicationDbContext.Database.EnsureCreated();
	}
}
