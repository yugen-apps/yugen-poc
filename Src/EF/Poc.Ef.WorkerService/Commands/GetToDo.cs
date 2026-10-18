using System;
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

public class GetToDo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly IQueryHandler<GetToDoQuery, TodoItem> _handler;
    private readonly ILogger<GetToDo> _logger;

    public GetToDo(
        IAnsiConsole console,
        IQueryHandler<GetToDoQuery, TodoItem> handler,
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

        var query = new GetToDoQuery(id);

        var result = await _handler.Handle(query, cancellationToken);
        if (result.IsSuccess)
        {
            LogHelper.Log(_console, result);
        }

        _console.PressAnyKey();

        return 0;
    }
}
