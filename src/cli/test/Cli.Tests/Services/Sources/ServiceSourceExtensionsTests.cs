using System;
using System.Collections.Generic;
using Cli.Services.Configuration;
using Cli.Services.Sources;
using Cli.Tests.Helpers;
using Xunit;

namespace Cli.Tests.Services.Sources
{
    public class ServiceSourceExtensionsTests
    {
        [Theory]
        [InlineData(SourceType.Docker)]
        [InlineData(null)]
        public void GetSource_ThrowsOnInvalidSource(SourceType? type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetSource());
        }

        [Fact]
        public void GetSource_ThrowsOnNotSupportedSource()
        {
            var source = new ServiceSource { Type = (SourceType)69 };

            Assert.Throws<NotSupportedException>(() => source.GetSource());
        }

        [Fact]
        public void GetSource_GetsDockerBuildSource()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerBuild,
                BuildContext = "context",
                Tag = "tag",
            };

            var result = source.GetSource();

            var (_, buildContext, tag) = Assert.IsType<DockerBuildSource>(result);
            Assert.Equal(source.BuildContext, buildContext);
            Assert.Equal(source.Tag, tag);
        }

        [Fact]
        public void GetSource_GetsDockerImageSource()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerImage,
                ImageName = "image",
                Tag = "tag",
            };

            var result = source.GetSource();

            var (_, imageName, tag) = Assert.IsType<DockerImageSource>(result);
            Assert.Equal(source.ImageName, imageName);
            Assert.Equal(source.Tag, tag);
        }

        [Fact]
        public void GetSource_GetsDotnetToolSource()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "tool",
                ExtraArgs = "args"
            };

            var result = source.GetSource();

            var (_ ,toolName, extraArgs) = Assert.IsType<DotnetToolSource>(result);
            Assert.Equal(source.ToolName, toolName);
            Assert.Equal(source.ExtraArgs, extraArgs);
        }

        [Fact]
        public void GetSource_GetsGitSource()
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = "https://example.com/repo.git"
            };

            var result = source.GetSource();

            var git = Assert.IsType<GitSource>(result);
            Assert.Equal(source.CloneUrl, git.CloneUrl);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept))]
        public void TryGetSource_GetsInvalidSource(SourceType? type)
        {
            var source = new ServiceSource { Type = type };

            var result = source.TryGetSource(out var converted);
            
            Assert.False(result);
            var (serviceSource, errors) = Assert.IsType<InvalidSource>(converted);
            Assert.Equal(source, serviceSource);
            Assert.NotEmpty(errors);
        }
        
        private static IEnumerable<object[]> SourceTypeValuesExcept() => SourceTypeValues.Except((SourceType)69);
    }
}
