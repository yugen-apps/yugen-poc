using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Poc.Identity.Application.Extensions;
using Poc.Identity.Blazor.Extensions;
using Poc.Identity.Infrastructure.Extensions;
using Poc.Identity.Persistence.Extensions;
using System.Threading.Tasks;

namespace Poc.Identity.Blazor;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add MudBlazor services
        builder.Services.AddMudServices();

        // Add services to the container.
        builder.Services.AddRazorComponents()
               .AddInteractiveServerComponents();

        // Add Services
        builder.Services.AddApplication();
        builder.Services.AddIdentity(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);

        var app = builder.Build();

        app.ConfigureServer(builder.Configuration);

        await app.InitializeDatabaseAsync();

        app.Run();
    }
}
