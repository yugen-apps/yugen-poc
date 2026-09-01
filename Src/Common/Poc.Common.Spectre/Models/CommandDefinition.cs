using System;
using System.Collections.Generic;

namespace Poc.Common.Spectre.Models;

public class CommandDefinition
{
    public CommandDefinition(string name)
    {
        Name = name;
    }

    public string? Alias { get; set; }

    public string? Description { get; set; }

    public List<CommandDefinitionExamples>? Examples { get; set; }

    public string Name { get; set; }

    public Type? Type { get; set; }
}
