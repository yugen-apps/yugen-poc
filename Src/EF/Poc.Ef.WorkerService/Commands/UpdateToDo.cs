using Microsoft.Extensions.Logging;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using Poc.Ef.Persistence.Commands;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class UpdateToDo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<UpdateToDoCommand, TodoItem> _handler;
	private readonly ILogger<UpdateToDo> _logger;

	public UpdateToDo(
		IAnsiConsole console,
		ICommandHandler<UpdateToDoCommand, TodoItem> handler,
		ILogger<UpdateToDo> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("Update...");

		var id = _console.Prompt(
			new TextPrompt<int>("Insert Id:"));

		var title = _console.Prompt(
			new TextPrompt<string>("Insert Title:"));

		var command = new UpdateToDoCommand(id, title);

		var result = await _handler.Handle(command, cancellationToken);
		if (result.IsSuccess)
		{
			LogHelper.Log(_console, result);
		}

		return 0;
	}
}
