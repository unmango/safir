using Moq;
using Moq.AutoMock;
using Safir.Manager.Configuration;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Safir.Manager.Tests.Configuration
{
    public class ConfigurationExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IConfiguration> _configuration;

        public ConfigurationExtensionsTests()
        {
            _configuration = _mocker.GetMock<IConfiguration>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSelfContained_ChecksConfiguredValues(bool expected)
        {
            _configuration.Setup(x => x["IsSelfContained"])
                .Returns(expected.ToString);

            var result = _configuration.Object.IsSelfContained();
            
            Assert.Equal(expected, result);
        }
    }
}
