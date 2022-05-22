using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Safir.Cli.Configuration;

internal sealed class JsonConfiguration : ILocalConfiguration
{
    private readonly IUserConfigurationFile _configurationFile;
    private readonly ILogger<JsonConfiguration> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new() {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        WriteIndented = true,
    };

    public JsonConfiguration(
        IUserConfigurationFile configurationFile,
        ILogger<JsonConfiguration> logger)
    {
        _configurationFile = configurationFile;
        _logger = logger;
    }

    public async ValueTask UpdateAsync(Action<LocalConfiguration> update, CancellationToken cancellationToken = default)
    {
        LocalConfiguration configuration;
        if (_configurationFile.Exists) {
            _logger.LogTrace("Reading existing configuration file");
            var reader = _configurationFile.GetReader();
            var onDisk = await JsonSerializer.DeserializeAsync<LocalConfiguration>(
                reader,
                _serializerOptions,
                cancellationToken);

            configuration = onDisk ?? new(new List<AgentOptions>());
        }
        else {
            _logger.LogTrace("Creating new configuration object");
            configuration = new(new List<AgentOptions>());
        }

        _logger.LogTrace("Performing configuration update");
        update(configuration);

        _logger.LogTrace("Writing updated configuration file {File}", options.File);
        await using var writeStream = _file.Create(options.File);
        await JsonSerializer.SerializeAsync(
            writeStream,
            configuration,
            _serializerOptions,
            cancellationToken);
    }
}
