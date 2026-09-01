using Ef.Poc.Application.Extensions;
using Ef.Poc.Infrastructure.Extensions;
using Ef.Poc.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ef.Poc.Api;

public class Program
{
    // dotnet ef migrations add v1 --project .\Ef.Poc.Persistence\ --startup-project .\Ef.Poc.Api\
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddControllers();

        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        //builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
        //builder.Services.AddTransient<ICategoryAppService, CategoriesAppService>();

        var app = builder.Build();


        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;

        //    var context = services.GetRequiredService<ApplicationDbContext>();
        //    context.Database.EnsureCreated();
        //}

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
