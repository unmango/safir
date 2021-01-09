using System;
using System.Collections.Generic;
using Cli.Services;
using Cli.Services.Installation;
using Cli.Services.Installation.Installers;
using Cli.Tests.Helpers;
using Moq;
using Xunit;

namespace Cli.Tests.Services
{
    public class ServiceInstallerFactoryExtensionsTests
    {
        private readonly Mock<IServiceInstallerFactory> _factory = new();

        [Theory]
        [InlineData(SourceType.Docker)]
        [InlineData(SourceType.DockerImage)]
        public void GetInstaller_GetsDockerImageInstaller(SourceType type)
        {
            var source = new ServiceSource {
                Type = type,
                ImageName = "image",
            };
            var expected = source with { Type = SourceType.DockerImage };

            _ = _factory.Object.GetInstaller(source);

            _factory.Verify(x => x.GetDockerImageInstaller(expected));
        }

        [Theory]
        [InlineData(SourceType.Docker)]
        [InlineData(SourceType.DockerBuild)]
        public void GetInstaller_GetsDockerBuildInstaller(SourceType type)
        {
            var source = new ServiceSource {
                Type = type,
                BuildContext = "context",
            };
            var expected = source with { Type = SourceType.DockerBuild };

            _ = _factory.Object.GetInstaller(source);

            _factory.Verify(x => x.GetDockerBuildInstaller(expected));
        }

        [Fact]
        public void GetInstaller_GetsGitInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = "a url",
            };

            _ = _factory.Object.GetInstaller(source);

            _factory.Verify(x => x.GetGitInstaller(source));
        }

        [Fact]
        public void GetInstaller_GetsDotnetToolInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "tool",
            };

            _ = _factory.Object.GetInstaller(source);

            _factory.Verify(x => x.GetDotnetToolInstaller(source));
        }

        [Fact]
        public void GetInstaller_GetsLocalDirectoryInstaller()
        {
            var source = new ServiceSource { Type = SourceType.LocalDirectory };

            _ = _factory.Object.GetInstaller(source);

            _factory.Verify(x => x.GetLocalDirectoryInstaller(source));
        }

        [Fact]
        public void GetInstaller_ThrowsWhenTypeIsNotSet()
        {
            var source = new ServiceSource { Type = null };

            Assert.Throws<InvalidOperationException>(() => _factory.Object.GetInstaller(source));
        }

        [Fact]
        public void GetInstaller_ThrowsWhenTypeIsUnrecognized()
        {
            var source = new ServiceSource { Type = (SourceType)69 };

            Assert.Throws<NotSupportedException>(() => _factory.Object.GetInstaller(source));
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.Docker, SourceType.DockerBuild, SourceType.DockerImage)]
        public void GetDockerInstaller_RequiresDockerSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => _factory.Object.GetDockerInstaller(source));
        }

        [Fact]
        public void GetDockerInstaller_InfersDockerBuildInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Docker,
                BuildContext = "context",
            };
            var expected = source with { Type = SourceType.DockerBuild };

            _ = _factory.Object.GetDockerInstaller(source);

            _factory.Verify(x => x.GetDockerBuildInstaller(expected));
        }

        [Fact]
        public void GetDockerInstaller_InfersDockerImageInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Docker,
                ImageName = "image",
            };
            var expected = source with { Type = SourceType.DockerImage };

            _ = _factory.Object.GetDockerInstaller(source);

            _factory.Verify(x => x.GetDockerImageInstaller(expected));
        }

        // MemberData doesn't support params, so this is fine for now.
        private static IEnumerable<object[]> SourceTypeValuesExcept(
            SourceType type1,
            SourceType type2,
            SourceType type3)
            => SourceTypeValues.Except(type1, type2, type3);
    }
}
