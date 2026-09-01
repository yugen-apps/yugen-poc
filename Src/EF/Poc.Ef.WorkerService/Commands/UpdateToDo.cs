using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poc.Ef.Application.Commands;
using Poc.Ef.Domain.Entities;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using Yugen.Common.Data.Commands;
using Yugen.Common.Spectre.Extensions;

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

	public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
	{
		_console.WriteLine("UpdateToDo...");

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

		_console.PressAnyKey();
		
		return 0;
	}
}
