using System.Text.Json;

namespace Poc.Shared.Extensions;

public static class JsonElementExtensions
{
	public static JsonElement? TryGetPropertyOrNull(this JsonElement jsonElement, string key)
	{
		if (jsonElement.TryGetProperty(key, out var p))
		{
			return p;
		}
		return null;
	}

	public static string? TryGetString(this JsonElement jsonElement, string key)
	{
		if (jsonElement.TryGetProperty(key, out var p))
		{
			if (p.ValueKind == JsonValueKind.String)
			{
				return p.GetString();
			}
		}
		return null;
	}

	public static int? TryGetInt(this JsonElement jsonElement, string key)
	{
		if (jsonElement.TryGetProperty(key, out var p))
		{
			if (p.ValueKind == JsonValueKind.Number)
			{
				return p.GetInt32();
			}
		}
		return null;
	}

	public static bool IsString(this JsonElement jsonElement, string key)
	{
		if (jsonElement.TryGetProperty(key, out var p))
		{
			return p.ValueKind == JsonValueKind.String;
		}
		return false;
	}

	public static bool IsNumber(this JsonElement jsonElement, string key)
	{
		if (jsonElement.TryGetProperty(key, out var p))
		{
			return p.ValueKind == JsonValueKind.Number;
		}
		return false;
	}
}
