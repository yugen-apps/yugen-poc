using Microsoft.Extensions.Logging;
using Poc.Identity.Application.Features.ToDoItems.Commands;
using Poc.Identity.Application.Features.ToDoItems.DTOs;
using Poc.Identity.WorkerService.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yugen.Common.Data.Cqrs;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Identity.WorkerService.Commands;

public class UpdateToDo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICommandHandler<UpdateToDoCommand, ToDoItemDto> _handler;
    private readonly ILogger<UpdateToDo> _logger;

    public UpdateToDo(
        IAnsiConsole console,
        ICommandHandler<UpdateToDoCommand, ToDoItemDto> handler,
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
            new TextPrompt<Guid>("Insert Id:"));

        var title = _console.Prompt(
            new TextPrompt<string>("Insert Title:"));

        UpdateToDoCommand command = new(id, title);

        var result = await _handler.Handle(command, cancellationToken);
        if (result.IsSuccess)
        {
            LogHelper.Log(_console, result);
        }

        _console.PressAnyKey();

        return 0;
    }
}
