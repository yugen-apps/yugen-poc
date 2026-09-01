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
    private readonly TypeResolver _customTypeResolver;

    private CancellationTokenSource _cts = new();

    public DefaultCommand(
        IAnsiConsole console,
        TypeResolver customTypeResolver)
    {
        _console = console;
        _customTypeResolver = customTypeResolver;
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

            var commandDefinition = _console.Prompt(
                   new SelectionPrompt<CommandDefinition>()
                       .Title("What [green]command[/] do you want to run?")
                       .AddChoices(CommandRegistry.GetCommands().Values)
                       .UseConverter(commandDefinition => $"{commandDefinition.Name} - {commandDefinition.Description}")
                       );

            _console.WriteLine(commandDefinition.Name);
            _console.Clear();

            if (_customTypeResolver.Resolve(commandDefinition.Name) is ICommand command)
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

