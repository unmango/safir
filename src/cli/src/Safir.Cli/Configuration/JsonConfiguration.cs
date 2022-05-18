using System;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Safir.Cli.Configuration;

internal sealed class JsonConfiguration<T> : ILocalConfiguration<T>
    where T : class, new()
{
    private readonly IOptionsMonitor<SafirOptions> _optionsMonitor;
    private readonly IDirectory _directory;
    private readonly IFile _file;
    private readonly ILogger<JsonConfiguration<T>> _logger;
    private readonly JsonSerializerOptions _serializerOptions = new() {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
        WriteIndented = true,
    };

    public JsonConfiguration(
        IOptionsMonitor<SafirOptions> optionsMonitor,
        IDirectory directory,
        IFile file,
        ILogger<JsonConfiguration<T>> logger)
    {
        _optionsMonitor = optionsMonitor;
        _directory = directory;
        _file = file;
        _logger = logger;
    }

    public async ValueTask UpdateAsync(Action<T> update, CancellationToken cancellationToken = default)
    {
        var options = _optionsMonitor.CurrentValue.Config;

        if (!_directory.Exists(options.Directory)) {
            _directory.CreateDirectory(options.Directory);
            _logger.LogTrace("Creating configuration directory {Directory}", options.Directory);
        }

        T configuration;
        if (_file.Exists(options.File)) {
            _logger.LogTrace("Reading existing configuration file {File}", options.File);
            await using var readStream = _file.OpenRead(options.File);
            var onDisk = await JsonSerializer.DeserializeAsync<T>(
                readStream,
                _serializerOptions,
                cancellationToken);

            configuration = onDisk ?? new();
        }
        else {
            _logger.LogTrace("Creating new configuration object");
            configuration = new();
        }

        _logger.LogTrace("Performing configuration update");
        update(configuration);

        _logger.LogTrace("Writing updated configuration file {File}", options.File);
        await using var writeStream = _file.OpenWrite(options.File);
        await JsonSerializer.SerializeAsync(
            writeStream,
            configuration,
            _serializerOptions,
            cancellationToken);
    }
}
