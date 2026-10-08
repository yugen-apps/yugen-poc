using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Common.Data.Extensions;
using Poc.Ef.Application.Extensions;
using Poc.Ef.Infrastructure.Extensions;
using Poc.Ef.Persistence.Contexts;

namespace Poc.Ef.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddApplication(builder.Configuration);
		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddPersistence<ApplicationDbContext>(builder.Configuration);

		builder.Services.AddControllers();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
