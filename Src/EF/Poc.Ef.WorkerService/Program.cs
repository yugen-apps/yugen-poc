using Microsoft.Extensions.Hosting;
using Poc.Common.Spectre.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Common.Spectre.Helpers;
using Poc.Ef.Application.Extensions;
using Poc.Ef.Infrastructure.Extensions;
using Poc.Ef.Persistence.Extensions;
using Poc.Ef.Persistence.Helpers;
using Poc.Ef.WorkerService.Commands;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add Services
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        // Add commands
        builder.Services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
        builder.Services.AddCommand<AddCommand>("Add");
        builder.Services.AddCommand<GetAllCommand>("GetAll");
        builder.Services.AddCommand<GetCommand>("Get");
        builder.Services.AddCommand<UpdateCommand>("Update");
        builder.Services.AddCommand<DeleteCommand>("Delete");
        builder.Services.AddCommand<ExitCommand>("Exit", cmd => { cmd.WithDescription("A command that exit the app"); });

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

        app.Services.EnsureCreated();

        await app.RunAsync();

        SqliteInMemoryHelper.Dispose();
    }
}
