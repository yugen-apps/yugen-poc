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

public class GetToDo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<GetToDoCommand, TodoItem> _handler;
	private readonly ILogger<GetToDo> _logger;

	public GetToDo(
		IAnsiConsole console,
		ICommandHandler<GetToDoCommand, TodoItem> handler,
		ILogger<GetToDo> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("Add...");

		var id = _console.Prompt(
			new TextPrompt<int>("Insert Id:"));

		var command = new GetToDoCommand(id);

		var result = await _handler.Handle(command, cancellationToken);
		if (result.IsSuccess)
		{
			LogHelper.Log(_console, result);
		}

		_console.PressAnyKey();

		return 0;
	}
}
