using Microsoft.Extensions.Options;
using Moq.AutoMock;
using Safir.Messaging.Configuration;
using Safir.Redis.Configuration;
using Xunit;

namespace Safir.Messaging.Tests.Configuration
{
    public class ConfigureRedisOptionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly MessagingOptions _messagingOptions = new();
        private readonly ConfigureRedisOptions _configureOptions;

        public ConfigureRedisOptionsTests()
        {
            var mockOptions = _mocker.GetMock<IOptions<MessagingOptions>>();
            mockOptions.SetupGet(x => x.Value).Returns(_messagingOptions);
            _configureOptions = _mocker.CreateInstance<ConfigureRedisOptions>();
        }

        [Fact]
        public void SetsRedisConfiguration()
        {
            const string connectionString = "connection string";
            _messagingOptions.ConnectionString = connectionString;
            var redisOptions = new RedisOptions();

            _configureOptions.Configure(redisOptions);
            
            Assert.Equal(connectionString, redisOptions.Configuration);
        }
    }
}
