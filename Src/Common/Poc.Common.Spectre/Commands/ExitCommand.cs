using Poc.Common.Spectre.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Common.Spectre.Commands;

public class ExitCommand : AsyncCommand
{
    public static CommandDefinition CommandDefinition => new("Exit");

    private readonly IAnsiConsole _console;

    public ExitCommand(
        IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Exiting...");

        return 1;
    }
}