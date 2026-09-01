using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poc.Ef.Application.Queries;
using Poc.Ef.Domain.Entities;
using Poc.Ef.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using Yugen.Common.Data.Commands;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Ef.WorkerService.Commands;

public class GetAllToDo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly IQueryHandler<GetAllToDoQuery, List<TodoItem>> _handler;
	private readonly ILogger<GetAllToDo> _logger;

	public GetAllToDo(
		IAnsiConsole console,
		IQueryHandler<GetAllToDoQuery, List<TodoItem>> handler,
		ILogger<GetAllToDo> logger)
	{
		_console = console;
		_handler = handler;
		_logger = logger;
	}

	public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
	{
		_console.WriteLine("GetAllToDo...");

		var query = new GetAllToDoQuery();

		var result = await _handler.Handle(query, cancellationToken);
		if (result.IsSuccess)
		{
			LogHelper.Log(_console, result);
		}

		_console.PressAnyKey();
		
		return 0;
	}
}
