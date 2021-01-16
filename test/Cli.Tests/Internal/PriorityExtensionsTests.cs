using System;
using Cli.Internal;
using Cli.Services;
using Cli.Tests.Helpers;
using Xunit;

namespace Cli.Tests.Internal
{
    public class PriorityExtensionsTests
    {
        [Fact]
        public void OrderByPriority_OrdersByPriority()
        {
            var sources = new[] {
                new TestConcreteServiceSource() { Priority = 1 },
                new TestConcreteServiceSource { Priority = 0 },
            };

            var ordered = sources.OrderByPriority();
            
            Assert.Collection(
                ordered,
                x => Assert.Equal(0, x.Priority),
                x => Assert.Equal(1, x.Priority));
        }

        // TODO: Fix this test
        // [Fact]
        // public void OrderByPriority_OrdersByPriorityWhenPriorityNotSet()
        // {
        //     var sources = new[] {
        //         new TestConcreteServiceSource { Priority = null },
        //         new TestConcreteServiceSource { Priority = 0 },
        //     };
        //
        //     var ordered = sources.OrderByPriority();
        //     
        //     Assert.Collection(
        //         ordered,
        //         x => Assert.Equal(0, x.Priority),
        //         x => Assert.False(x.Priority.HasValue));
        // }

        [Fact]
        public void HighestPriority_ReturnsHighestPriority()
        {
            var sources = new[] {
                new TestConcreteServiceSource { Priority = 1 },
                new TestConcreteServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriority();
            
            Assert.Equal(0, hightest.Priority);
        }

        [Fact]
        public void HighestPriority_ReturnsHighestPriorityMatchingPredicate()
        {
            const string expected = "expected";
            var sources = new[] {
                new TestConcreteServiceSource(expected) { Priority = 1 },
                new TestConcreteServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriority(x => x.Name == expected);
            
            Assert.Equal(1, hightest.Priority);
        }

        // TODO: Review is this is functionality I want...
        [Fact]
        public void HighestPriority_ThrowsWhenNoSources()
        {
            var sources = Array.Empty<IServiceSource>();

            Assert.Throws<InvalidOperationException>(() => sources.HighestPriority());
        }

        // TODO: Review is this is functionality I want...
        // TODO: Also fix this test if I do want it
        // [Fact]
        // public void HighestPriority_ThrowsWhenNoSourcesMatchPredicate()
        // {
        //     var sources = new[] { new TestConcreteServiceSource { Priority = 0 } };
        //
        //     Assert.Throws<InvalidOperationException>(
        //         () => sources.HighestPriority(x => !string.IsNullOrWhiteSpace(x.CloneUrl)));
        // }

        [Fact]
        public void HighestPriorityOrDefault_ReturnsHighestPriority()
        {
            var sources = new[] {
                new TestConcreteServiceSource { Priority = 1 },
                new TestConcreteServiceSource { Priority = 0 },
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
                new TestConcreteServiceSource(expected) { Priority = 1 },
                new TestConcreteServiceSource { Priority = 0 },
            };

            var hightest = sources.HighestPriorityOrDefault(x => x.Name == expected);
            
            Assert.NotNull(hightest);
            Assert.Equal(1, hightest!.Priority);
        }

        [Fact]
        public void HighestPriority_ReturnsNullWhenNoSources()
        {
            var sources = Array.Empty<IServiceSource>();

            var highest = sources.HighestPriorityOrDefault();
            
            Assert.Null(highest);
        }

        // TODO: Fix this test. Also relates to review above I believe
        // [Fact]
        // public void HighestPriority_ReturnsNullWhenNoSourcesMatchPredicate()
        // {
        //     var sources = new[] { new TestConcreteServiceSource { Priority = 0 } };
        //
        //     var highest = sources.HighestPriorityOrDefault(x => !string.IsNullOrWhiteSpace(x.CloneUrl));
        //     
        //     Assert.Null(highest);
        // }
    }
}
