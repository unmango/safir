using System;
using System.Collections.Generic;
using Cli.Internal.Wrappers.Git;
using Cli.Services;
using Cli.Services.Installation.Installers;
using Cli.Tests.Helpers;
using FluentValidation;
using Moq;
using Xunit;

namespace Cli.Tests.Services.Installation
{
    public class DefaultServiceInstallerFactoryTests
    {
        private readonly Mock<IRepositoryFunctions> _repositoryFunctions = new();
        private readonly Mock<IRemoteFunctions> _remoteFunctions = new();
        private readonly Mock<IServiceProvider> _services = new();
        private readonly DefaultServiceInstallerFactory _factory;
        
        public DefaultServiceInstallerFactoryTests()
        {
            _services.Setup(x => x.GetService(typeof(IRepositoryFunctions))).Returns(_repositoryFunctions.Object);
            _services.Setup(x => x.GetService(typeof(IRemoteFunctions))).Returns(_remoteFunctions.Object);
            
            _factory = new DefaultServiceInstallerFactory(_services.Object);
        }
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DockerBuild)]
        public void GetDockerBuildInstaller_RequiresDockerBuildSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<ValidationException>(() => _factory.GetDockerBuildInstaller(source));
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void GetDockerBuildInstaller_RequiresBuildContext(string? buildContext)
        {
            var source = new ServiceSource {
                Type = SourceType.DockerBuild,
                BuildContext = buildContext,
            };

            Assert.Throws<ValidationException>(() => _factory.GetDockerBuildInstaller(source));
        }

        [Fact]
        public void GetDockerBuildInstaller_GetsDockerBuildInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerBuild,
                BuildContext = "context",
            };

            var result = _factory.GetDockerBuildInstaller(source);

            Assert.IsType<DockerBuildInstaller>(result);
        }
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DockerImage)]
        public void GetDockerImageInstaller_RequiresDockerBuildSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<ValidationException>(() => _factory.GetDockerImageInstaller(source));
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void GetDockerImageInstaller_RequiresImageName(string? imageName)
        {
            var source = new ServiceSource {
                Type = SourceType.DockerImage,
                ImageName = imageName,
            };

            Assert.Throws<ValidationException>(() => _factory.GetDockerImageInstaller(source));
        }

        [Fact]
        public void GetDockerImageInstaller_GetsDockerImageInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerImage,
                ImageName = "image",
            };

            var result = _factory.GetDockerImageInstaller(source);

            Assert.IsType<DockerImageInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.Git)]
        public void GetGitInstaller_RequiresGitSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<ValidationException>(() => _factory.GetGitInstaller(source));
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void GetGitInstaller_RequiresCloneUrl(string? cloneUrl)
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = cloneUrl,
            };

            Assert.Throws<ValidationException>(() => _factory.GetGitInstaller(source));
        }

        [Fact]
        public void GetGitInstaller_GetsGitInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = "https://example.com/repo.git",
            };
            _remoteFunctions.Setup(x => x.IsValidName(It.IsAny<string>())).Returns(true);

            var result = _factory.GetGitInstaller(source);

            Assert.IsType<GitInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DotnetTool)]
        public void GetDotnetToolInstaller_RequiresDotnetToolSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<ValidationException>(() => _factory.GetDotnetToolInstaller(source));
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void GetDotnetToolInstaller_RequiresToolName(string? toolName)
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = toolName,
            };

            Assert.Throws<ValidationException>(() => _factory.GetDotnetToolInstaller(source));
        }

        [Fact]
        public void GetDotnetToolInstaller_GetsDotnetToolInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "name",
            };

            var result = _factory.GetDotnetToolInstaller(source);

            Assert.IsType<DotnetToolInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.LocalDirectory)]
        public void GetLocalDirectoryInstaller_RequiresLocalDirectorySourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<ValidationException>(() => _factory.GetLocalDirectoryInstaller(source));
        }

        [Fact]
        public void GetLocalDirectoryInstaller_GetsNoOpInstaller()
        {
            var source = new ServiceSource { Type = SourceType.LocalDirectory };

            var result = _factory.GetLocalDirectoryInstaller(source);

            Assert.IsType<NoOpInstaller>(result);
        }

        private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type) => SourceTypeValues.Except(type);
    }
}
