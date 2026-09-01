using Microsoft.Extensions.Hosting;
using Poc.Common.Spectre;
using Poc.Common.Spectre.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.ConsoleApp.Commands;
using Poc.ConsoleApp.Commands.Grouping;
using Spectre.Console.Cli;
using System.Threading;
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
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSpectre();

        var registrar = new TypeRegistrar(builder.Services);

        var commandApp = new CommandApp<DefaultCommand>(registrar);

        commandApp.Configure(config =>
        {
            config.PropagateExceptions();

            config.AddCommandRegistryCommand<HelloWorldCommand>(
                HelloWorldCommand.CommandDefinition);

            config.AddCommandRegistryCommand<GroupingCommand>(
                GroupingCommand.CommandDefinition);

            config.AddCommandRegistryCommand<TaskCommand>(
                TaskCommand.CommandDefinition);

            config.AddCommandRegistryCommand<ExitCommand>(
                ExitCommand.CommandDefinition);
        });

        return await commandApp.RunAsync(args, CancellationToken.None);
    }
}
