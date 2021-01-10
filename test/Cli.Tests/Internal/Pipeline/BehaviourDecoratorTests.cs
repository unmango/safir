using System;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;
using Moq;
using Xunit;

namespace Cli.Tests.Internal.Pipeline
{
    public class BehaviourDecoratorTests
    {
        private readonly Mock<IPipelineBehaviour<object>> _decorated = new();
        private readonly Mock<IPipelineBehaviour<object>> _decorator = new();
        private readonly BehaviourDecorator<object> _behaviourDecorator;

        public BehaviourDecoratorTests()
        {
            _behaviourDecorator = new BehaviourDecorator<object>(
                _decorated.Object,
                _decorator.Object);
        }

        [Fact]
        public void AppliesTo_ByDefaultWontOverrideDecorated()
        {
            var context = new object();
            _decorated.Setup(x => x.AppliesTo(context)).Returns(true);
            _decorator.Setup(x => x.AppliesTo(context)).Returns(false);

            var result = _behaviourDecorator.AppliesTo(context);

            Assert.True(result);
            _decorated.Verify(x => x.AppliesTo(context));
            _decorator.Verify(x => x.AppliesTo(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void AppliesTo_OverridesDecoratedWhenFlagIsSet()
        {
            var context = new object();
            _decorated.Setup(x => x.AppliesTo(context)).Returns(false);
            _decorator.Setup(x => x.AppliesTo(context)).Returns(true);
            var behaviourDecorator = new BehaviourDecorator<object>(
                _decorated.Object,
                _decorator.Object,
                overrideAppliesTo: true);

            var result = behaviourDecorator.AppliesTo(context);

            Assert.True(result);
            _decorated.Verify(x => x.AppliesTo(It.IsAny<object>()), Times.Never);
            _decorator.Verify(x => x.AppliesTo(context));
        }

        [Fact]
        public async Task InvokeAsync_InvokesDecoratorWithDecoratedAsNext()
        {
            var context = new object();
            var count = 0;
            _decorator.Setup(x => x.InvokeAsync(
                    context,
                    It.IsAny<Func<object, ValueTask>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<object, Func<object, ValueTask>, CancellationToken>(
                    async (_, next, _) => {
                        count++;
                        await next(context);
                    });
            _decorated.Setup(x => x.InvokeAsync(
                    context,
                    It.IsAny<Func<object, ValueTask>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<object, Func<object, ValueTask>, CancellationToken>(
                    async (_, next, _) => {
                        Assert.Equal(1, count++);
                        await next(context);
                    });

            await _behaviourDecorator.InvokeAsync(context, _ => ValueTask.CompletedTask);
            
            Assert.Equal(2, count);
            _decorated.VerifyAll();
            _decorator.VerifyAll();
        }
    }
}
