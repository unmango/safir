using System;
using System.Threading.Tasks;
using Cli.Internal.Wrappers.Process;
using Cli.Services.Process;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Cli.Tests.Services.Process
{
    public class ProcessServiceTests
    {
        private const string FileName = "file";
        private static readonly string[]_args = { "arg1", "arg2" };
        private readonly Mock<IProcessFactory> _processFactory = new();
        private readonly Mock<IProcess> _process = new();
        private readonly Mock<IOptions<ConfigOptions>> _options = new();
        private readonly Mock<ILogger<ProcessService>> _logger = new();

        private readonly ProcessService _service;

        public ProcessServiceTests()
        {
            _options.SetupGet(x => x.Value).Returns(new ConfigOptions());

            _processFactory.Setup(x => x.Create(It.IsAny<ProcessArguments>()))
                .Returns(_process.Object);
            
            _service = new ProcessService(
                _processFactory.Object,
                _options.Object,
                _logger.Object,
                FileName,
                _args);
        }

        [Fact]
        public void CtorThrowsWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ProcessService(
                null!,
                _options.Object,
                _logger.Object,
                string.Empty,
                Array.Empty<string>()));
        }

        [Fact]
        public void CtorThrowsWhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ProcessService(
                _processFactory.Object,
                null!,
                _logger.Object,
                string.Empty,
                Array.Empty<string>()));
        }

        [Fact]
        public async Task StartSetsFileName()
        {
            await _service.StartAsync();
            
            _processFactory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.StartInfo != null && args.StartInfo.FileName == FileName)));
        }

        [Fact]
        public async Task StartSetsArguments()
        {
            const string expected = "arg1 arg2";
            
            await _service.StartAsync();
            
            _processFactory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.StartInfo != null && args.StartInfo.Arguments == expected)));
        }

        [Fact]
        public async Task StartStartsProcess()
        {
            await _service.StartAsync();
            
            _process.Verify(x => x.Start());
        }

        [Theory]
        [InlineData(true, 0)]
        [InlineData(false, 1)]
        public async Task StartConvertsResult(bool returns, int expected)
        {
            _process.Setup(x => x.Start()).Returns(returns);
            
            var task = _service.StartAsync();

            var result = await Assert.IsType<Task<int>>(task);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task StartSetsStartedProcessId()
        {
            await _service.StartAsync();
            
            // TODO: How am I even gonna set it
        }
    }
}
