using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Safir.Manager.Configuration;
using Xunit;

namespace Safir.Manager.Tests.Configuration
{
    public class ConfigurationExtensionsTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSelfContained_ChecksConfiguredValues(bool expected)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["SelfContained"] = expected.ToString()
                })
                .Build();
            
            var result = configuration.IsSelfContained();
            
            Assert.Equal(expected, result);
        }
    }
}
