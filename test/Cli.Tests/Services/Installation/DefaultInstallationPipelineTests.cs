using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cli.Services;
using Cli.Services.Configuration;
using Cli.Services.Installation;
using Cli.Tests.Helpers;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Cli.Tests.Services.Installation
{
    public class DefaultInstallationPipelineTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IInstallationMiddleware> _installer1 = new();
        private readonly Mock<IInstallationMiddleware> _installer2 = new();
        private readonly DefaultInstallationPipeline _pipeline;

        private readonly InstallationContext _context = new(
            string.Empty,
            new DefaultService("Name", new List<IServiceSource>()),
            new[] { new TestConcreteServiceSource() });

        public DefaultInstallationPipelineTests()
        {
            _installer1.Setup(x => x.AppliesTo(It.IsAny<InstallationContext>())).Returns(true);
            _installer2.Setup(x => x.AppliesTo(It.IsAny<InstallationContext>())).Returns(true);
            _mocker.Use<IEnumerable<IInstallationMiddleware>>(new[] {
                _installer1.Object,
                _installer2.Object,
            });

            _pipeline = _mocker.Get<DefaultInstallationPipeline>();
        }

        [Fact]
        public async Task InvokesApplicableInstaller()
        {
            _installer1.Setup(x => x.AppliesTo(_context)).Returns(true);

            await _pipeline.InstallAsync(_context);

            _installer1.Verify(x => x.InvokeAsync(
                _context,
                It.IsAny<Func<InstallationContext, ValueTask>>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task InvokesAllApplicableInstallers()
        {
            // Setup "next" call
            _installer1.Setup(x => x.InvokeAsync(
                    _context,
                    It.IsAny<Func<InstallationContext, ValueTask>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<InstallationContext, Func<InstallationContext, ValueTask>, CancellationToken>(
                    async (context, next, _) => await next(context));
            
            _installer1.Setup(x => x.AppliesTo(_context)).Returns(true);
            _installer2.Setup(x => x.AppliesTo(_context)).Returns(true);

            await _pipeline.InstallAsync(_context);

            _installer1.Verify(x => x.InvokeAsync(
                _context,
                It.IsAny<Func<InstallationContext, ValueTask>>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task SkipsInapplicableInstaller()
        {
            _installer1.Setup(x => x.AppliesTo(_context)).Returns(false);
            _installer2.Setup(x => x.AppliesTo(_context)).Returns(true);

            await _pipeline.InstallAsync(_context);

            _installer1.Verify(x => x.InvokeAsync(
                    It.IsAny<InstallationContext>(),
                    It.IsAny<Func<InstallationContext, ValueTask>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _installer2.Verify(x => x.InvokeAsync(
                _context,
                It.IsAny<Func<InstallationContext, ValueTask>>(),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task ShortCircuitsWhenNoApplicableInstallers()
        {
            _installer1.Setup(x => x.AppliesTo(_context)).Returns(false);
            _installer2.Setup(x => x.AppliesTo(_context)).Returns(false);

            await _pipeline.InstallAsync(_context);

            _installer1.Verify(x => x.InvokeAsync(
                    It.IsAny<InstallationContext>(),
                    It.IsAny<Func<InstallationContext, ValueTask>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            _installer2.Verify(x => x.InvokeAsync(
                    It.IsAny<InstallationContext>(),
                    It.IsAny<Func<InstallationContext, ValueTask>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
