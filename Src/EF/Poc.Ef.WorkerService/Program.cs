using Microsoft.Extensions.Hosting;
using Poc.Ef.Application.Extensions;
using Poc.Ef.Infrastructure.Extensions;
using Poc.Ef.Persistence.Contexts;
using Poc.Ef.WorkerService.Extensions;
using Spectre.Console.Cli;
using System.Threading.Tasks;
using Yugen.Common.Data;
using Yugen.Common.Data.Extensions;
using Yugen.Common.Data.Helpers;
using Yugen.Common.Spectre.Commands;
using Yugen.Common.Spectre.Extensions;
using Yugen.Common.Spectre.Helpers;

namespace Poc.Ef.WorkerService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add Services
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistenceFactory<ApplicationDbContext, ApplicationDbContextFactory>(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);

        // The standard call save for the commands will be pre-added & configured
        builder.UseSpectreConsole<DefaultCommand>(config =>
        {
            // All commands above are passed to config.AddCommand() by this point
#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
            config.UseBasicExceptionHandler();
        });

        var app = builder.Build();

        //await app.EnsureCreatedAsync<ApplicationDbContext>();
        await app.EnsureCreatedFactoryAsync<IApplicationDbContextFactory>();

        await app.RunAsync();

        SqliteInMemoryHelper.Dispose();
    }
}
