using System.Text.Json.Serialization;

namespace Safir.Cli.Configuration;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
[JsonSerializable(typeof(LocalConfiguration))]
internal partial class JsonGeneratorContext : JsonSerializerContext
{
}
