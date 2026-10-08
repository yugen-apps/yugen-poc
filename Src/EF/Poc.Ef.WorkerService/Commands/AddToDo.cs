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

public class AddToDo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<AddToDoCommand, TodoItem> _handler;
	private readonly ILogger<AddToDo> _logger;

	public AddToDo(
		IAnsiConsole console,
		ICommandHandler<AddToDoCommand, TodoItem> handler,
		ILogger<AddToDo> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;

		//using var scope = _serviceProvider.CreateScope();
		//_categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("Add...");

		var title = _console.Prompt(
			new TextPrompt<string>("Insert Title:"));

		var command = new AddToDoCommand(title);

		var result = await _handler.Handle(command, cancellationToken);
		if (result.IsSuccess)
		{
			LogHelper.Log(_console, result);
		}

		_console.PressAnyKey();

		return 0;
	}
}
