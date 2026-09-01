using Microsoft.Extensions.Hosting;
using Poc.Identity.Application.Extensions;
using Poc.Identity.Infrastructure.Extensions;
using Poc.Identity.Persistence.Contexts;
using Poc.Identity.Persistence.Extensions;
using Poc.Identity.WorkerService.Extensions;
using Spectre.Console.Cli;
using System.Threading.Tasks;
using Yugen.Common.Identity.Core.Extensions;
using Yugen.Common.Persistence;
using Yugen.Common.Persistence.Extensions;
using Yugen.Common.Persistence.Helpers;
using Yugen.Common.Spectre.Commands;
using Yugen.Common.Spectre.Extensions;
using Yugen.Common.Spectre.Helpers;

namespace Poc.Identity.WorkerService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add Services
        builder.Services.AddApplication();
        builder.Services.AddIdentityCore<ApplicationDbContext>(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);
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
