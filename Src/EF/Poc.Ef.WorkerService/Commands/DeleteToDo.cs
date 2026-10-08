using Microsoft.Extensions.Logging;
using Poc.Common.Data.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Ef.Domain.Entities;
using Poc.Ef.Persistence.Commands;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class DeleteToDo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<DeleteToDoCommand> _handler;
	private readonly ILogger<DeleteToDo> _logger;

	public DeleteToDo(
		IAnsiConsole console,
		ICommandHandler<DeleteToDoCommand> handler,
		ILogger<DeleteToDo> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("Delete...");

		var id = _console.Prompt(
			new TextPrompt<int>("Insert Id:"));

		var command = new DeleteToDoCommand(id);

		var result = await _handler.Handle(command, cancellationToken);

		_console.WriteLine($"IsSuccess...{result.IsSuccess}");

		_console.PressAnyKey();

		return 0;
	}
}
