using Poc.Common.Spectre.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.Common.Spectre.Commands;

public class DefaultCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;
    private readonly CommandRegistry _commandRegistry;

    private CancellationTokenSource _cts = new();

    public DefaultCommand(
        IAnsiConsole console,
        CommandRegistry commandRegistry)
    {
        _console = console;
        _commandRegistry = commandRegistry;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken _)
    {
        Console.CancelKeyPress += OnCancelKeyPress;
        var result = 0;

        while (result == 0)
        {
            var figlet = new FigletText("Menu")
                 .Centered()
                 .Color(Color.Purple);
            _console.Write(figlet);

            //foreach (var enricher in _console.Profile.Enrichers)
            //{
            //    _console.MarkupLine($"[blue]Debug enricher:[/] {enricher}");
            //}

            var commandRegistration = _console.Prompt(
                   new SelectionPrompt<CommandRegistration>()
                       .Title("What [green]command[/] do you want to run?")
                       .AddChoices(_commandRegistry.GetCommands())
                       .UseConverter(commandRegistration => $"{commandRegistration.Name}")
                       );

            _console.WriteLine(commandRegistration.Name);
            _console.Clear();

            if (_commandRegistry.GetCommand(commandRegistration) is ICommand command)
            {
                result = await command.ExecuteAsync(context, null!, _cts.Token);
            }
        }

        return result;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        // Prevent the application from terminating
        e.Cancel = true;

        _cts.Cancel();

        _cts = new CancellationTokenSource();
    }
}

