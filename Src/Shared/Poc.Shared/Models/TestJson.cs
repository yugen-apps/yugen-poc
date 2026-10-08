using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poc.Shared.Models;

public class TestJson
{
	[JsonPropertyName("name")]
	public string? Name { get; set; }

	[JsonPropertyName("stringArrayAsString")]
	public string? StringArrayAsString { get; set; }

	[JsonPropertyName("intArrayAsString")]
	public string? IntArrayAsString { get; set; }

	//[JsonPropertyName("test")]
	//public JsonNode TestNode { get; set; }

	//[JsonPropertyName("test")]
	//public JsonArray TestArray { get; set; }

	//[JsonPropertyName("test")]
	//public JsonObject TestObject { get; set; }

	[JsonPropertyName("testString")]
	public JsonElement TestString { get; set; }

	[JsonPropertyName("testInt")]
	public JsonElement TestInt { get; set; }

	[JsonPropertyName("testArray")]
	public JsonElement TestArray { get; set; }

	[JsonPropertyName("properties")]
	public JsonElement Properties { get; set; }
}