using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yugen.Common;
using Yugen.Common.Web;

namespace Poc.Ef.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
	public static void ConfigureServices(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddScoped<SystemInfoService>();
		services.AddScoped<WebHostInfoService>();
	}
}
