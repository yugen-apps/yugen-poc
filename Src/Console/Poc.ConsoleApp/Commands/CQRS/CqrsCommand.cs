using Poc.ConsoleApp.Commands.CQRS;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands;

public class CqrsCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly ICommandHandler<CompleteTodoCommand> _handler;

    public CqrsCommand(
        IAnsiConsole console, 
        ICommandHandler<CompleteTodoCommand> handler)
    {
        _console = console;
        _handler = handler;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var command = new CompleteTodoCommand(1);

        Result result = await _handler.Handle(command, cancellationToken);        

        _console.WriteLine($"result: {result.IsSuccess} ...");

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }
}