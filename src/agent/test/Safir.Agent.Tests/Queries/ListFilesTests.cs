using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using Safir.Agent.Domain;
using Safir.Agent.Queries;
using Xunit;

namespace Safir.Agent.Tests.Queries
{
    public class ListFilesTests
    {
        private readonly AutoMocker _mock = new();
        private readonly IRequestHandler<ListFilesRequest, ListFilesResponse> _handler;

        public ListFilesTests()
        {
            _handler = _mock.CreateInstance<ListFilesHandler>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public async Task SendsEmptyWhenNoRoot(string? root)
        {
            var request = new ListFilesRequest(root!);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Empty(result.Files);
            _mock.GetMock<IDirectory>().VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SendsEmptyWhenDirectoryDoesNotExist()
        {
            const string dir = "dir";
            _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(false);
            var request = new ListFilesRequest(dir);

            var result = await _handler.Handle(request, default);

            Assert.NotNull(result);
            Assert.Empty(result.Files);
            _mock.GetMock<IDirectory>().VerifyAll();
            _mock.GetMock<IDirectory>().VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EnumeratesWithWildCardFilter()
        {
            const string dir = "dir";
            _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
            var request = new ListFilesRequest(dir);

            await _handler.Handle(request, default);

            _mock.GetMock<IDirectory>()
                .Verify(x => x.EnumerateFileSystemEntries(dir, "*", It.IsAny<EnumerationOptions>()));
        }

        [Fact]
        public async Task PassesEnumerationOptionsFromRequest()
        {
            const string dir = "dir";
            _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
            var options = new EnumerationOptions();
            var request = new ListFilesRequest(dir, options);

            await _handler.Handle(request, default);

            _mock.GetMock<IDirectory>().Verify(x => x.EnumerateFileSystemEntries(
                dir,
                It.IsAny<string>(),
                It.Is<EnumerationOptions>(y => y == options)));
        }

        [Fact]
        public async Task ReturnsPathRelativeToRoot()
        {
            const string dir = "dir", entry = "entry", relative = "relative";
            _mock.Setup<IDirectory, bool>(x => x.Exists(dir)).Returns(true);
            _mock.Setup<IDirectory, IEnumerable<string>>(x =>
                    x.EnumerateFileSystemEntries(dir, It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
                .Returns(new[] { entry });
            _mock.Setup<IPath, string>(x => x.GetRelativePath(dir, entry)).Returns(relative);
            var request = new ListFilesRequest(dir);

            var result = await _handler.Handle(request, default);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.Files);
            var paths = result.Files.Select(x => x.Path).ToList();
            Assert.Contains(relative, paths);
            Assert.DoesNotContain(entry, paths);
        }
    }
}
