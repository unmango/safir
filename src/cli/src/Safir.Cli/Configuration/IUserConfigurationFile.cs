using System.IO.Pipelines;

namespace Safir.Cli.Configuration;

public interface IUserConfigurationFile
{
    bool Exists { get; }

    PipeReader GetReader();

    PipeWriter GetWriter();
}
