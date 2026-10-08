using Poc.Common;
using Poc.Common.Data.Commands;
using Poc.Common.Spectre.Extensions;
using Poc.Ef.Persistence.Commands;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Ef.WorkerService.Commands;

public class CompleteTodo : AsyncCommand
{
	private readonly IAnsiConsole _console;
	private readonly ICommandHandler<CompleteTodoCommand> _handler;

	public CompleteTodo(
		IAnsiConsole console,
		ICommandHandler<CompleteTodoCommand> handler)
	{
		_console = console;
		_handler = handler;
	}

	public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
	{
		var command = new CompleteTodoCommand(1);

		Result result = await _handler.Handle(command, cancellationToken);

		_console.WriteLine($"result: {result.IsSuccess} ...");

		_console.PressAnyKey();

		return 0;
	}
}