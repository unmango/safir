using System;
using System.Collections.Generic;
using System.Linq;
using Cli.Services;
using Cli.Services.Installers;
using Xunit;

namespace Cli.Tests.Services
{
    public class ServiceSourceExtensionsTests
    {
        [Theory]
        [InlineData(SourceType.Docker)]
        [InlineData(SourceType.DockerImage)]
        public void GetInstaller_GetsDockerImageInstaller(SourceType type)
        {
            var source = new ServiceSource {
                Type = type,
                ImageName = "image",
            };

            var result = source.GetInstaller();

            Assert.IsType<DockerImageInstaller>(result);
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

            var result = source.GetInstaller();

            Assert.IsType<DockerBuildInstaller>(result);
        }
        
        [Fact]
        public void GetInstaller_GetsGitInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = "a url",
            };

            var result = source.GetInstaller();

            Assert.IsType<GitInstaller>(result);
        }

        [Fact]
        public void GetInstaller_GetsDotnetToolInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "tool",
            };

            var result = source.GetInstaller();

            Assert.IsType<DotnetToolInstaller>(result);
        }

        [Fact]
        public void GetInstaller_GetsLocalDirectoryInstaller()
        {
            var source = new ServiceSource { Type = SourceType.LocalDirectory };

            var result = source.GetInstaller();

            Assert.IsType<NoOpInstaller>(result);
        }

        [Fact]
        public void GetInstaller_ThrowsWhenTypeIsNotSet()
        {
            var source = new ServiceSource { Type = null };

            Assert.Throws<InvalidOperationException>(() => source.GetInstaller());
        }

        [Fact]
        public void GetInstaller_ThrowsWhenTypeIsUnrecognized()
        {
            var source = new ServiceSource { Type = (SourceType)69 };

            Assert.Throws<NotSupportedException>(() => source.GetInstaller());
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.Docker)]
        public void GetDockerInstaller_RequiresDockerSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetDockerInstaller());
        }

        [Fact]
        public void GetDockerInstaller_InfersDockerBuildInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Docker,
                BuildContext = "context",
            };

            var result = source.GetDockerInstaller();

            Assert.IsType<DockerBuildInstaller>(result);
        }

        [Fact]
        public void GetDockerInstaller_InfersDockerImageInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Docker,
                ImageName = "image",
            };

            var result = source.GetDockerInstaller();

            Assert.IsType<DockerImageInstaller>(result);
        }
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DockerBuild)]
        public void GetDockerBuildInstaller_RequiresDockerBuildSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetDockerBuildInstaller());
        }

        [Theory]
        [MemberData(nameof(NullOrWhitespaceStrings))]
        public void GetDockerBuildInstaller_RequiresBuildContext(string? buildContext)
        {
            var source = new ServiceSource {
                Type = SourceType.DockerBuild,
                BuildContext = buildContext,
            };

            Assert.Throws<InvalidOperationException>(() => source.GetDockerBuildInstaller());
        }

        [Fact]
        public void GetDockerBuildInstaller_GetsDockerBuildInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerBuild,
                BuildContext = "context",
            };

            var result = source.GetDockerBuildInstaller();

            Assert.IsType<DockerBuildInstaller>(result);
        }
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DockerImage)]
        public void GetDockerImageInstaller_RequiresDockerBuildSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetDockerImageInstaller());
        }

        [Theory]
        [MemberData(nameof(NullOrWhitespaceStrings))]
        public void GetDockerImageInstaller_RequiresImageName(string? imageName)
        {
            var source = new ServiceSource {
                Type = SourceType.DockerImage,
                ImageName = imageName,
            };

            Assert.Throws<InvalidOperationException>(() => source.GetDockerImageInstaller());
        }

        [Fact]
        public void GetDockerImageInstaller_GetsDockerImageInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DockerImage,
                ImageName = "image",
            };

            var result = source.GetDockerImageInstaller();

            Assert.IsType<DockerImageInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.Git)]
        public void GetGitInstaller_RequiresGitSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetGitInstaller());
        }

        [Theory]
        [MemberData(nameof(NullOrWhitespaceStrings))]
        public void GetGitInstaller_RequiresCloneUrl(string? cloneUrl)
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = cloneUrl,
            };

            Assert.Throws<InvalidOperationException>(() => source.GetGitInstaller());
        }

        [Fact]
        public void GetGitInstaller_GetsGitInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = "a url",
            };

            var result = source.GetGitInstaller();

            Assert.IsType<GitInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DotnetTool)]
        public void GetDotnetToolInstaller_RequiresDotnetToolSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetDotnetToolInstaller());
        }

        [Theory]
        [MemberData(nameof(NullOrWhitespaceStrings))]
        public void GetDotnetToolInstaller_RequiresToolName(string? toolName)
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = toolName,
            };

            Assert.Throws<InvalidOperationException>(() => source.GetDotnetToolInstaller());
        }

        [Fact]
        public void GetDotnetToolInstaller_GetsDotnetToolInstaller()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "name",
            };

            var result = source.GetDotnetToolInstaller();

            Assert.IsType<DotnetToolInstaller>(result);
        }

        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.LocalDirectory)]
        public void GetLocalDirectoryInstaller_RequiresLocalDirectorySourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            Assert.Throws<InvalidOperationException>(() => source.GetLocalDirectoryInstaller());
        }

        [Fact]
        public void GetLocalDirectoryInstaller_GetsNoOpInstaller()
        {
            var source = new ServiceSource { Type = SourceType.LocalDirectory };

            var result = source.GetLocalDirectoryInstaller();

            Assert.IsType<NoOpInstaller>(result);
        }

        [Fact]
        public void OrderByPriority_OrdersByPriority()
        {
            var sources = new[] {
                new ServiceSource { Priority = 1 },
                new ServiceSource { Priority = 0 },
            };

            var ordered = sources.OrderByPriority();
            
            Assert.Collection(
                ordered,
                x => Assert.Equal(0, x.Priority),
                x => Assert.Equal(1, x.Priority));
        }

        [Fact]
        public void OrderByPriority_OrdersByPriorityWhenPriorityNotSet()
        {
            var sources = new[] {
                new ServiceSource { Priority = null },
                new ServiceSource { Priority = 0 },
            };

            var ordered = sources.OrderByPriority();
            
            Assert.Collection(
                ordered,
                x => Assert.Equal(0, x.Priority),
                x => Assert.False(x.Priority.HasValue));
        }

        [Fact]
        public void HighestPriority_ReturnsHighestPriority()
        {
            var sources = new[] {
                new ServiceSource { Priority = 1 },
                new ServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriority();
            
            Assert.Equal(0, hightest.Priority);
        }

        [Fact]
        public void HighestPriority_ReturnsHighestPriorityMatchingPredicate()
        {
            const string expected = "expected";
            var sources = new[] {
                new ServiceSource { Priority = 1, CloneUrl = expected },
                new ServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriority(x => x.CloneUrl == expected);
            
            Assert.Equal(1, hightest.Priority);
        }

        // TODO: Review is this is functionality I want...
        [Fact]
        public void HighestPriority_ThrowsWhenNoSources()
        {
            var sources = Array.Empty<ServiceSource>();

            Assert.Throws<InvalidOperationException>(() => sources.HighestPriority());
        }

        // TODO: Review is this is functionality I want...
        [Fact]
        public void HighestPriority_ThrowsWhenNoSourcesMatchPredicate()
        {
            var sources = new[] { new ServiceSource { Priority = 0 } };

            Assert.Throws<InvalidOperationException>(
                () => sources.HighestPriority(x => !string.IsNullOrWhiteSpace(x.CloneUrl)));
        }

        [Fact]
        public void HighestPriorityOrDefault_ReturnsHighestPriority()
        {
            var sources = new[] {
                new ServiceSource { Priority = 1 },
                new ServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriorityOrDefault();
            
            Assert.NotNull(hightest);
            Assert.Equal(0, hightest!.Priority);
        }

        [Fact]
        public void HighestPriorityOrDefault_ReturnsHighestPriorityMatchingPredicate()
        {
            const string expected = "expected";
            var sources = new[] {
                new ServiceSource { Priority = 1, CloneUrl = expected },
                new ServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriorityOrDefault(x => x.CloneUrl == expected);
            
            Assert.NotNull(hightest);
            Assert.Equal(1, hightest!.Priority);
        }

        [Fact]
        public void HighestPriority_ReturnsNullWhenNoSources()
        {
            var sources = Array.Empty<ServiceSource>();

            var highest = sources.HighestPriorityOrDefault();
            
            Assert.Null(highest);
        }

        [Fact]
        public void HighestPriority_ReturnsNullWhenNoSourcesMatchPredicate()
        {
            var sources = new[] { new ServiceSource { Priority = 0 } };

            var highest = sources.HighestPriorityOrDefault(x => !string.IsNullOrWhiteSpace(x.CloneUrl));
            
            Assert.Null(highest);
        }

        [Fact]
        public void InferSourceType_ReturnsTypeWhenSet()
        {
            const SourceType expected = (SourceType)69;
            var source = new ServiceSource { Type = expected };

            var inferred = source.InferSourceType();
            
            Assert.Equal(expected, inferred);
        }

        [Fact]
        public void InferSourceType_InfersDockerBuild()
        {
            const SourceType expected = SourceType.DockerBuild;
            var source = new ServiceSource { BuildContext = "context" };

            var inferred = source.InferSourceType();
            
            Assert.Equal(expected, inferred);
        }

        [Fact]
        public void InferSourceType_InfersDockerImage()
        {
            const SourceType expected = SourceType.DockerImage;
            var source = new ServiceSource { ImageName = "image" };

            var inferred = source.InferSourceType();
            
            Assert.Equal(expected, inferred);
        }

        [Fact]
        public void InferSourceType_InfersGit()
        {
            const SourceType expected = SourceType.Git;
            var source = new ServiceSource { CloneUrl = "url" };

            var inferred = source.InferSourceType();
            
            Assert.Equal(expected, inferred);
        }

        [Fact]
        public void InferSourceType_ReturnsNullWhenUnableToInfer()
        {
            var inferred = new ServiceSource().InferSourceType();
            
            Assert.Null(inferred);
        }

        [Fact]
        public void InferSourceType_SetsTypeOnOutParameter()
        {
            const SourceType expected = SourceType.Git;
            var source = new ServiceSource { CloneUrl = "url" };

            _ = source.InferSourceType(out var updated);
            
            Assert.Equal(expected, updated.Type);
            Assert.NotEqual(source, updated);
        }

        [Fact]
        public void TryInferSourceType_ReturnsTrueWhenSourceWasInferred()
        {
            const SourceType expected = SourceType.Git;
            var source = new ServiceSource { CloneUrl = "url" };

            var result = source.TryInferSourceType(out var inferred);
            
            Assert.True(result);
            Assert.Equal(expected, inferred);
        }

        [Fact]
        public void TryInferSourceType_ReturnsNullWhenUnableToInfer()
        {
            var result = new ServiceSource().TryInferSourceType(out var inferred);
            
            Assert.False(result);
        }

        private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type)
        {
            return Enum.GetValues<SourceType>()
                .Except(new[] { type })
                .Concat(new[] { (SourceType)69 })
                .Select(x => new object[] { x });
        }

        private static IEnumerable<object[]> NullOrWhitespaceStrings()
        {
            return new[] {
                null,
                "",
                " ",
                "\t",
                "\n",
            }.Select(x => new object[] { x });
        }
    }
}
