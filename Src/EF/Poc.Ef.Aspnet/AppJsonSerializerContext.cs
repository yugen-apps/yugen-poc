using Poc.Common;
using Poc.Ef.Aspnet.Models;
using System.Text.Json.Serialization;

namespace Poc.Ef.Aspnet;

[JsonSerializable(typeof(EnvironmentInfo))]
[JsonSerializable(typeof(Operation))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
