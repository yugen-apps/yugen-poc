using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poc.Identity.Application.Features.ToDoItems.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using Yugen.Common.Persistence.Cqrs;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Identity.WorkerService.Commands;

public class CompleteTodo : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICommandHandler<CompleteTodoCommand> _handler;
    private readonly ILogger<CompleteTodo> _logger;

    public CompleteTodo(
        IAnsiConsole console,
        ICommandHandler<CompleteTodoCommand> handler,
        ILogger<CompleteTodo> logger)
    {
        _console = console;
        _handler = handler;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
    {
        _console.WriteLine("CompleteTodo...");

        CompleteTodoCommand command = new(new Guid("1"));

        var result = await _handler.Handle(command, cancellationToken);

        _console.WriteLine($"result: {result.IsSuccess} ...");

        _console.PressAnyKey();

        return 0;
    }
}