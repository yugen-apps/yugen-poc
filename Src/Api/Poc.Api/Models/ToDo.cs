using System;

namespace Poc.Api.Models;

public class ToDo
{
    public ToDo(int id, Guid owner, string description)
    {
        Id = id;
        Owner = owner;
        Description = description;
    }

    public int Id { get; set; }

    public Guid Owner { get; set; }

    public string Description { get; set; } = string.Empty;
}
