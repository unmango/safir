using System;
using Cli.Services;
using Cli.Services.Configuration;
using Cli.Tests.Helpers;
using Xunit;

namespace Cli.Tests.Services.Configuration
{
    public class ServiceSourceExtensionsTests
    {
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
            var result = new ServiceSource().TryInferSourceType(out _);
            
            Assert.False(result);
        }
    }
}
