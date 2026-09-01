using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Yugen.Common.Spectre.Commands;

public class ExitCommand : AsyncCommand
{
	private readonly IAnsiConsole _console;

	public ExitCommand(
		IAnsiConsole console)
	{
		_console = console;
	}

	public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
	{
		_console.WriteLine("Exiting...");

		return 1;
	}
}