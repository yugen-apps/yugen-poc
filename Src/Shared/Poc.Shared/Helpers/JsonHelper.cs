using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Poc.Shared.Helpers;

public class JsonHelper
{
	public static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
	{
		AllowTrailingCommas = true,
		ReadCommentHandling = JsonCommentHandling.Skip,
		NumberHandling =
				JsonNumberHandling.AllowReadingFromString |
				JsonNumberHandling.WriteAsString,
		WriteIndented = true
	};

	public static async Task<T?> Get<T>(string fileName)
	{
		var result = await ReadTextAsync(fileName);
		return JsonSerializer.Deserialize<T>(result, JsonSerializerOptions);
	}

	public static async Task<string> ReadTextAsync(string filePath)
	{
		string text = await File.ReadAllTextAsync(filePath);
		return text;
	}

	public static async Task<string> Set<T>(T value)
	{
		return JsonSerializer.Serialize(value, JsonSerializerOptions);
	}
}
