using System.Text.Json;

namespace Entra.Blazor.Models
{
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
}