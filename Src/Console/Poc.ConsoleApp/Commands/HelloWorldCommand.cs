using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands;


[Description("Says 'Hello' to you and optionally your dog.")]
public class HelloWorldCommandSettings : CommandSettings
{
    [Description("Your Name")]
    [CommandArgument(0, "<name>")]
    [CommandOption("-n|--name")]
    //[Required]
    public string? Name { get; set; }

    [Description("Your Dogs Name (Optional)")]
    [CommandArgument(1, "[name]")]
    [CommandOption("-p|--pup")]
    public string? DogsName { get; set; }
}

public class HelloWorldCommand : AsyncCommand<HelloWorldCommandSettings>
{
    private readonly IAnsiConsole _console;

    public HelloWorldCommand(
        IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, HelloWorldCommandSettings settings, CancellationToken cancellationToken)
    {
        if (settings?.Name == null)
        {
            settings = new HelloWorldCommandSettings
            {
                Name = _console.Prompt(
                    new TextPrompt<string>("What [green]name[/] ?"))
            };
            _console.WriteLine(settings.Name);
        }

        _console.MarkupLineInterpolated($"[darkseagreen2_1] Hello {settings.Name}![/]");

        if (!string.IsNullOrEmpty(settings.DogsName))
        {
            _console.MarkupLineInterpolated($"[darkseagreen2_1] Ooooo who's a good pup? {settings.DogsName} thats who! 🐶[/]");
        }

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }
}
