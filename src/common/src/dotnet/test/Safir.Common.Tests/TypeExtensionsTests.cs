using Xunit;

namespace Safir.Common.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsAssignableToGeneric_ReturnsTrueWhenConcreteIsClosedGenericOpenInterface()
        {
            var @interface = typeof(IOpen<>);
            var closed = typeof(Closed);

            var result = closed.IsAssignableToGeneric(@interface);
            
            Assert.True(result);
        }
        
        // ReSharper disable once UnusedTypeParameter
        private interface IOpen<T> { }
        
        private class Closed : IOpen<int> { }
    }
}
