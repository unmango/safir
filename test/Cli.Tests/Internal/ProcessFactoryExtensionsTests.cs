using System;
using System.Diagnostics;
using Cli.Internal;
using Moq;
using Xunit;

namespace Cli.Tests.Internal
{
    public class ProcessFactoryExtensionsTests
    {
        private readonly Mock<IProcess> _process = new();
        private readonly Mock<IProcessFactory> _factory = new();

        public ProcessFactoryExtensionsTests()
        {
            _factory.Setup(x => x.Create(It.IsAny<ProcessArguments>()))
                .Returns(_process.Object);
        }

        [Fact]
        public void CreateWithIdThrowsWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ProcessFactoryExtensions.Create(null!, default(int)));
        }

        [Fact]
        public void CreateWithIdPassesId()
        {
            const int id = 69;

            _factory.Object.Create(id);
            
            _factory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.Id == id)));
            _factory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.StartInfo == null)));
        }

        [Fact]
        public void CreateProcessWithConfigureThrowWhenConfigureIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _factory.Object.Create((Action<ProcessStartInfo>)null!));
        }

        [Fact]
        public void CreateProcessWithConfigureConfiguresStartInfo()
        {
            const string fileName = "file";

            _factory.Object.Create(x => x.FileName = fileName);
            
            _factory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.Id == null)));
            _factory.Verify(x => x.Create(It.Is<ProcessArguments>(
                args => args.StartInfo != null
                        && args.StartInfo.FileName == fileName)));
        }
    }
}
