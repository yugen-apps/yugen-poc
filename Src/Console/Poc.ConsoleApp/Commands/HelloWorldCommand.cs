using Poc.Common.Spectre.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands;

public class HelloWorldCommand : AsyncCommand
{
    public static CommandDefinition CommandDefinition => new("Hello");

    private readonly IAnsiConsole _console;

    public HelloWorldCommand(
        IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Hello World");

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }
}
