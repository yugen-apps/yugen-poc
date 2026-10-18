using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Poc.Ef.Application.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using Yugen.Common.Data.Commands;
using Yugen.Common.Spectre.Extensions;

namespace Poc.Ef.WorkerService.Commands;

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

        var command = new CompleteTodoCommand(new Guid("1"));

        var result = await _handler.Handle(command, cancellationToken);

        _console.WriteLine($"result: {result.IsSuccess} ...");

        _console.PressAnyKey();

        return 0;
    }
}