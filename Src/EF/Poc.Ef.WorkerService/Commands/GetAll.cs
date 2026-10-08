using Microsoft.Extensions.Logging;
using Poc.Common.Data.Commands;
using Poc.Ef.Domain.Entities;
using Poc.Ef.Persistence.Commands;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class GetAll : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<GetAllToDoCommand, List<TodoItem>> _handler;
	private readonly ILogger<GetAll> _logger;

	public GetAll(
		IAnsiConsole console,
		ICommandHandler<GetAllToDoCommand, List<TodoItem>> handler,
		ILogger<GetAll> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		_console.WriteLine("GetAll...");

		var command = new GetAllToDoCommand();

		var result = await _handler.Handle(command, cancellationToken);
		if (result.IsSuccess)
		{
			LogHelper.Log(_console, result);
		}
		return 0;
	}
}
