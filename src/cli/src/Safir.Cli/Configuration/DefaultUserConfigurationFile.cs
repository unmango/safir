using System;
using System.IO.Abstractions;
using System.IO.Pipelines;
using Microsoft.Extensions.Options;

namespace Safir.Cli.Configuration;

internal sealed class DefaultUserConfigurationFile : IUserConfigurationFile
{
    private readonly IPath _path;
    private readonly IDirectory _directory;
    private readonly IFile _file;
    private readonly string _filePath;

    public DefaultUserConfigurationFile(IOptions<SafirOptions> options, IPath path, IDirectory directory, IFile file)
    {
        _path = path;
        _directory = directory;
        _file = file;
        _filePath = options.Value.Config.File;
    }

    public bool Exists => _file.Exists(_filePath);

    public PipeReader GetReader()
    {
        if (!Exists)
            throw new InvalidOperationException("Configuration file doesn't exist");

        var stream = _file.OpenRead(_filePath);
        return PipeReader.Create(stream);
    }

    public PipeWriter GetWriter()
    {
        if (_path.EndsInDirectorySeparator(_filePath))
            throw new InvalidOperationException("Invalid configuration file path");

        if (!_path.IsPathRooted(_filePath))
            throw new InvalidOperationException("Configuration file path must be absolute");

        var directory = _path.GetDirectoryName(_filePath);

        if (!_directory.Exists(directory))
            _directory.CreateDirectory(directory);

        var stream = _file.OpenWrite(_filePath);
        return PipeWriter.Create(stream);
    }
}
