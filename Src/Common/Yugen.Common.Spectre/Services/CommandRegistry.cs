using Yugen.Common.Spectre.Extensions;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using Yugen.Common.Spectre.Models;

// https://github.com/jakenuts/Community.Extensions.Spectre.Cli.Hosting
namespace Yugen.Common.Spectre.Services;

public class CommandRegistry
{
    private readonly IServiceProvider _serviceProvider;

    public CommandRegistry(
        IEnumerable<CommandRegistration> commands,
        IServiceProvider serviceProvider
    )
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<CommandRegistration> GetCommands() =>
        _serviceProvider.GetRegisteredCommands();

    public ICommand? GetCommand(CommandRegistration commandRegistration) =>
        _serviceProvider.GetService(commandRegistration.CommandType) as ICommand;
}