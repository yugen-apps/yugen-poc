using Microsoft.Extensions.Logging;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using Poc.Identity.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Data.Cqrs;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Identity.WorkerService.Commands;

public class GetAllToDo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> _handler;
    private readonly ILogger<GetAllToDo> _logger;

    public GetAllToDo(
        IAnsiConsole console,
        IQueryHandler<GetAllToDoItemsQuery, List<ToDoItemDto>> handler,
        ILogger<GetAllToDo> logger)
    {
        _console = console;
        _handler = handler;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
    {
        _console.WriteLine("GetAllToDo...");

        GetAllToDoItemsQuery query = new();

        var result = await _handler.Handle(query, cancellationToken);
        if (result.IsSuccess)
        {
            LogHelper.Log(_console, result);
        }

        _console.PressAnyKey();

        return 0;
    }
}
