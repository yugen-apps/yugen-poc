using Knx.Falcon.Configuration;
using Knx.Falcon.Discovery;
using Knx.Falcon.Sdk;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.Console;

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
        var commandApp = new CommandApp<DefaultCommand>();

        commandApp.Configure(config =>
        {
            config.PropagateExceptions();

            config.AddCommand<ListDevicesCommand>(nameof(ListDevicesCommand));
            config.AddCommand<LiveTableCommand>(nameof(LiveTableCommand));
            config.AddCommand<ConnectCommand>(nameof(ConnectCommand));
        });

        return await commandApp.RunAsync(args, CancellationToken.None);
    }
}

public class DefaultCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ListDevicesCommand _listDevicesCommand;
    private readonly LiveTableCommand _liveTableCommand;
    private readonly ConnectCommand _connectCommand;

    public DefaultCommand(
        IAnsiConsole console,
        ListDevicesCommand listDevicesCommand,
        LiveTableCommand liveTableCommand,
        ConnectCommand connectCommand)
    {
        _console = console;
        _listDevicesCommand = listDevicesCommand;
        _liveTableCommand = liveTableCommand;
        _connectCommand = connectCommand;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.MarkupLine("Welcome to [green]Dashboard.Console[/]!");
        int result = 0;

        while (result == 0)
        {
            _console.Clear();
            var figlet = new FigletText("Main Menu")
                         .Centered()
                         .Color(Color.Purple);
            _console.Write(figlet);
            _console.Write(new Rule());

            foreach (var enricher in _console.Profile.Enrichers)
            {
                _console.MarkupLine($"[blue]Debug enricher:[/] {enricher}");
            }

            var command = _console.Prompt(
            new SelectionPrompt<string>()
                .Title("What [green]command[/] do you want to run?")
                .AddChoices(
                    nameof(ListDevicesCommand),
                    nameof(LiveTableCommand),
                    nameof(ConnectCommand),
                    "Exit"
                ));

            _console.WriteLine(command);
            _console.Clear();

            result = command switch
            {
                //nameof(ListDevicesCommand) => await _listDevicesCommand.ExecuteAsync(context, CancellationToken.None),
                //nameof(LiveTableCommand) => await _liveTableCommand.ExecuteAsync(context, CancellationToken.None),
                //nameof(ConnectCommand) => await _connectCommand.ExecuteAsync(context, CancellationToken.None),
                _ => 1,
            };
        }

        return result;
    }
}

public class ListDevicesCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;

    public ListDevicesCommand(IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var table = new Table();
        table.AddColumn(nameof(IpDeviceDiscoveryResult.FriendlyName));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.ControlEndpoint));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.DiscoveryEndpoint));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.IndividualAddress));

        var devices = KnxBus.DiscoverIpDevicesAsync();

        // Status spinner for work
        await _console
              .Status()
              .StartAsync("Fetching devices...", async ctx =>
              {
                  await foreach (var device in devices)
                  {
                      table.AddRow(
                          device.FriendlyName,
                          $"{device.ControlEndpoint.Address}:{device.ControlEndpoint.Port}",
                          $"{device.DiscoveryEndpoint.Address}:{device.DiscoveryEndpoint.Port}",
                          device.IndividualAddress.ToString()
                          );
                  }
              });
        _console.Write(table);

        var prompt = new SelectionPrompt<IpDeviceDiscoveryResult>()
                    .Title("What [green]connection[/] do you want to run?");

        await foreach (IpDeviceDiscoveryResult device in devices)
        {
            prompt.AddChoice(device);
        }

        var selectedDevice = _console.Prompt(prompt);

        _console.WriteLine(selectedDevice.FriendlyName);

        // var connectionString = $"Type=IpTunneling;" +
        //                         $"HostAddress={device.ControlEndpoint.Address};" +
        //                         $"SerialNumber={device.SerialNumber};" +
        //                         $"MacAddress={device.MacAddress};" +
        //                         $"ProtocolType=Udp;" +
        //                         $"Name={device.FriendlyName}";
        // var connectorParameters = ConnectorParameters.FromConnectionString(command);
        // KnxBus bus = new(connectorParameters);

        var ipTunnelingConnectorParameters = new IpTunnelingConnectorParameters(selectedDevice.ControlEndpoint.Address.ToString(), selectedDevice.ControlEndpoint.Port);

        KnxBus bus = new(ipTunnelingConnectorParameters);

        await bus.ConnectAsync(cancellationToken);

        _console.WriteLine(bus.ConnectionState.ToString());

        var name = AnsiConsole.Ask<string>("...");

        return 0;
    }
}

public class LiveTableCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;

    public LiveTableCommand(IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var table = new Table();
        table.AddColumn(nameof(IpDeviceDiscoveryResult.FriendlyName));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.ControlEndpoint));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.DiscoveryEndpoint));
        table.AddColumn(nameof(IpDeviceDiscoveryResult.IndividualAddress));

        // Live updating table
        await _console
              .Live(table)
              .StartAsync(async ctx =>
              {
                  for (int i = 0; i < 5; i++)
                  {
                      await Task.Delay(500);
                      table.AddRow(
                          "Loading...",
                          "Loading...",
                          "Loading...",
                          "Loading..."
                      );
                      ctx.Refresh();
                  }
              });

        return 0;
    }
}
public class ConnectCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;

    public ConnectCommand(IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var ipTunnelingConnectorParameters = new IpTunnelingConnectorParameters("192.168.0.8");

        KnxBus bus = new(ipTunnelingConnectorParameters);

        await bus.ConnectAsync(cancellationToken);

        _console.WriteLine(bus.ConnectionState.ToString());

        var name = AnsiConsole.Ask<string>("...");

        return 0;
    }
}