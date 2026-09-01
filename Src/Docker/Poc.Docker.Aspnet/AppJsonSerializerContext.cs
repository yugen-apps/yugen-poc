using Poc.Docker.Aspnet.Models;
using System.Text.Json.Serialization;

namespace Poc.Docker.Aspnet;

[JsonSerializable(typeof(EnvironmentInfo))]
[JsonSerializable(typeof(Operation))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
