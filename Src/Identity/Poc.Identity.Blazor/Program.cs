using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Poc.Identity.Application.Extensions;
using Poc.Identity.Blazor.Extensions;
using Poc.Identity.Persistence.Contexts;
using System.Threading.Tasks;
using Yugen.Common.Data.Extensions;

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

        // builder.Services.AddHealthChecks();

        // Uncomment if using System.Text.Json source generation
        //builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

        // Add Services
        builder.Services.AddApplication();
        builder.Services.AddPersistenceFactory<ApplicationDbContext, ApplicationDbContextFactory>(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddIdentity(builder.Configuration);

        var app = builder.Build();

        app.ConfigureServer(builder.Configuration);
        await app.InitializeServerAsync(builder.Configuration);

        app.Run();
    }
}
