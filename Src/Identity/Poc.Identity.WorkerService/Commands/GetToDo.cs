using Microsoft.Extensions.Logging;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.Application.Features.ToDoItems.Queries;
using Poc.Identity.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Identity.WorkerService.Commands;

public class GetToDo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IQueryHandler<GetToDoQuery, ToDoItemDto> _handler;
    private readonly ILogger<GetToDo> _logger;

    public GetToDo(
        IAnsiConsole console,
        IQueryHandler<GetToDoQuery, ToDoItemDto> handler,
        ILogger<GetToDo> logger)
    {
        _console = console;
        _handler = handler;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
    {
        _console.WriteLine("GetToDo...");

        var id = _console.Prompt(
            new TextPrompt<Guid>("Insert Id:"));

        GetToDoQuery query = new(id);

        var result = await _handler.Handle(query, cancellationToken);
        if (result.IsSuccess)
        {
            LogHelper.Log(_console, result);
        }

        _console.PressAnyKey();

        return 0;
    }
}
