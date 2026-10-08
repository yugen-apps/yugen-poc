using Spectre.Console;

namespace Poc.Common.Spectre.Extensions;

public static class AnsiConsoleExtensions
{
	public static void PressAnyKey(this IAnsiConsole console)
	{
		console.Prompt(new TextPrompt<string>("... Press Any Key ...").AllowEmpty());
	}
}