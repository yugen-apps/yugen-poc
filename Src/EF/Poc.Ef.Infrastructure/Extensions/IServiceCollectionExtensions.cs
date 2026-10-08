using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.Ef.Infrastructure.Services;

namespace Poc.Ef.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddTransient<IEmailService, EmailService>();
	}
}