using System;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Cli.Tests.Internal.Pipeline
{
    public class PipelineBehaviourExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IPipelineBehaviour<object>> _behaviour;

        public PipelineBehaviourExtensionsTests()
        {
            _behaviour = _mocker.GetMock<IPipelineBehaviour<object>>();
        }

        [Fact]
        public async Task Decorate_DecoratesBehaviour()
        {
            // TODO: This test needs a once over...
            var flag = false;
            var context = new object();
            
            var decorated = _behaviour.Object.Decorate((_, next, _) => {
                flag = true;
                return next(context);
            });
            await decorated.InvokeAsync(new object(), _ => ValueTask.CompletedTask);

            Assert.True(flag);
            _behaviour.Verify(x => x.InvokeAsync(
                context,
                It.IsAny<Func<object, ValueTask>>(),
                It.IsAny<CancellationToken>()));
        }
    }
}
