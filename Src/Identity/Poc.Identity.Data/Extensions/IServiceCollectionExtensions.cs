using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Poc.Identity.Data.Extensions;

public static class IServiceCollectionExtensions
{
	public static void AddData(this IServiceCollection services, IConfiguration configuration)
	{
		//var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		//services.AddDbContext<ApplicationDbContext>(options =>
		//	options.UseSqlServer(connectionString));
	}
}