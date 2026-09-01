using System.Text.Json;

namespace Poc.Identity.Blazor.Models;

public class KeyValues
{
    public KeyValues(string name, object values)
    {
        Name = name;
        Values = JsonSerializer.Serialize(values, new JsonSerializerOptions { WriteIndented = true });
    }

    public string Name { get; set; }

    public string Values { get; set; }
}
