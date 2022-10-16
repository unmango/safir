using System.IO.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Safir.AspNetCore.IntegrationTesting;
using Safir.Grpc;
using FileSystem = Safir.Agent.Protos.FileSystem;

namespace Safir.Agent.IntegrationTests.Services;

public class FileSystemServiceTestsGrpc : IClassFixture<WebApplicationFactory<Program>>
{
    private const string DataDirectory = "Test";
    private readonly Mock<IDirectory> _directory = new();
    private readonly Mock<IPath> _path = new();
    private readonly FileSystem.FileSystemClient _client;

    public FileSystemServiceTestsGrpc(WebApplicationFactory<Program> factory)
    {
        var fileSystem = new Mock<IFileSystem>();
        fileSystem.SetupGet(x => x.Directory).Returns(_directory.Object);
        fileSystem.SetupGet(x => x.Path).Returns(_path.Object);

        factory = factory.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services => {
                services.Configure<AgentConfiguration>(o => o.DataDirectory = DataDirectory);
                services.AddTransient(_ => fileSystem.Object);
            });
        });

        var channel = factory.CreateChannel();
        _client = new FileSystem.FileSystemClient(channel);
    }

    [Fact]
    public async Task ListFiles_ListsFiles()
    {
        var entries = new[] { "test", "test2" };
        _directory.Setup(x => x.Exists(DataDirectory)).Returns(true);
        _directory.Setup(x => x.EnumerateFileSystemEntries(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(entries);
        _path.Setup(x => x.GetRelativePath(DataDirectory, "test")).Returns("test");
        _path.Setup(x => x.GetRelativePath(DataDirectory, "test2")).Returns("test2");

        var result = await _client.ListFiles(new()).ResponseStream.ToListAsync();

        Assert.Equal(entries, result.Select(x => x.Path));
    }
}
