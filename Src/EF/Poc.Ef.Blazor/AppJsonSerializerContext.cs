using Yugen.Common;
using Poc.Ef.Blazor.Models;
using System.Text.Json.Serialization;

namespace Poc.Ef.Blazor;

[JsonSerializable(typeof(EnvironmentInfo))]
[JsonSerializable(typeof(Operation))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
