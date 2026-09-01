using Spectre.Console.Cli;
using System;

namespace Poc.Common.Spectre.Models;

/// <summary>
///     A base registration class for commands with their types and name
/// </summary>
/// <param name="CommandType"></param>
/// <param name="Name"></param>
public abstract record CommandRegistration(Type CommandType, string Name)
{
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public abstract void Configure(IConfigurator configuration);
}