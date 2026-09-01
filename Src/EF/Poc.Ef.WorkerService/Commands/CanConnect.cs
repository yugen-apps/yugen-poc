using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poc.Ef.Application.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using Yugen.Common;
using Yugen.Common.Data.Commands;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Ef.WorkerService.Commands;

public class CanConnect : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<CanConnectCommand> _handler;
	private readonly ILogger<CanConnect> _logger;

	public CanConnect(
		IAnsiConsole console,
		ICommandHandler<CanConnectCommand> handler, 
		ILogger<CanConnect> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
	{
		_console.WriteLine("CanConnect...");

		var command = new CanConnectCommand();

		Result result = await _handler.Handle(command, cancellationToken);

		_console.WriteLine($"CanConnect: {result.IsSuccess} ...");

		_console.PressAnyKey();

		return 0;
	}
}