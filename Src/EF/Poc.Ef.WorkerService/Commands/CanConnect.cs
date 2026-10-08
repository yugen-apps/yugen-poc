using Poc.Common;
using Poc.Common.Data.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Ef.Persistence.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class CanConnect : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<CanConnectCommand> _handler;

	public CanConnect(
		IAnsiConsole console,
		ICommandHandler<CanConnectCommand> handler)
	{
		_console = console;
		_handler = handler;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("CanConnect...");

		var command = new CanConnectCommand();

		Result result = await _handler.Handle(command, cancellationToken);

		_console.WriteLine($"CanConnect: {result.IsSuccess} ...");

		_console.PressAnyKey();

		return 0;
	}
}