using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.Common.Spectre.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Common.Spectre.Helpers;
using Poc.ConsoleApp.Commands;
using Poc.ConsoleApp.Commands.Grouping;
using Poc.ConsoleApp.Commands.Lifetime;
using Spectre.Console.Cli;
using System;
using System.Threading.Tasks;

namespace Poc.ConsoleApp;

/*
docker build --tag app .
docker run --name app --interactive --tty app

# default bridge
docker run --name app --interactive --tty --network bridge --publish 3671:3671 app

docker run --name app --interactive --tty --network bridge --publish 3671:3671 --add-host=host.docker.internal:host-gateway app

# host
docker run --name app --interactive --tty --network host app

docker run --name app --interactive --tty --network host --rm app
   
# null
docker run --name app --interactive --tty --network null app

# macvlan
docker network create -d macvlan \
   --subnet=192.168.0.0/24 \
   --gateway=192.168.0.1 \
   -o parent=enxf46b8cdb0f4d \
   my_macvlan_net
      
docker run --name app --interactive --tty --network my_macvlan_net app   
*/
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Add Services
        builder.Services.AddTransient<IOperationTransient, Operation>();
        builder.Services.AddScoped<IOperationScoped, Operation>();
        builder.Services.AddSingleton<IOperationSingleton, Operation>();
        builder.Services.AddTransient<OperationService>();
        //builder.Services.AddSingleton<OperationService>();

        builder.Services.AddCommand<DefaultCommand>("Menu", cmd => { cmd.WithDescription("Default command that show the menu"); });
        builder.Services.AddCommand<HelloWorldCommand>("HelloWorld", cmd => { cmd.WithAlias("h"); });
        builder.Services.AddCommand<GroupingCommand>("Grouping");
        builder.Services.AddCommand<LifetimeCommand>("Lifetime");
        builder.Services.AddCommand<TaskCommand>("Task");
        builder.Services.AddCommand<ExitCommand>("Exit");

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

        await app.RunAsync();
    }
}
