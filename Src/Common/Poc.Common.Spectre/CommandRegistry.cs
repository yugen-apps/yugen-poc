using Poc.Common.Spectre.Models;
using Spectre.Console.Cli;
using System.Collections.Generic;

namespace Poc.Common.Spectre;

// https://github.com/Jlabarca/Ghost/blob/main/Ghost.Father/CLI/Commands/CommandRegistry.cs
// https://github.com/LukeFZ/XblContainerReader/blob/master/XblContainerReader/Program.cs
// https://github.com/jakenuts/Community.Extensions.Spectre.Cli.Hosting
public static class CommandRegistry
{
    private static readonly Dictionary<string, CommandDefinition> Commands =
    [
    ];

    public static void AddCommandRegistryCommand<TCommand>(
        this IConfigurator config,
        CommandDefinition commandDefinition,
        List<CommandDefinitionExamples>? examples = null)
        where TCommand : class, ICommand
    {
        commandDefinition.Type = typeof(TCommand);

        Commands.Add(commandDefinition.Name, commandDefinition);

        var command = config.AddCommand<TCommand>(commandDefinition.Name)
                            .WithAlias(commandDefinition.Alias!)
                            .WithDescription(commandDefinition.Description!);

        foreach (var example in examples ?? [])
        {
            command.WithExample(example.Args);
        }
    }

    public static Dictionary<string, CommandDefinition> GetCommands() => Commands;

    public static CommandDefinition? GetCommandDefinition(string name)
    {
        if (Commands.TryGetValue(name, out var commandDefinition))
        {
            return commandDefinition;
        }

        return null;
    }
}