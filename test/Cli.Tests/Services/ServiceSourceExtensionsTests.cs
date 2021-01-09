using System;
using Cli.Services;
using Xunit;

namespace Cli.Tests.Services
{
    public class ServiceSourceExtensionsTests
    {
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
            var result = new ServiceSource().TryInferSourceType(out _);
            
            Assert.False(result);
        }
    }
}
