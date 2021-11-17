using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Common;

namespace Safir.Serialization.Protobuf.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProtobufSerializer(this IServiceCollection services)
        {
            return services.AddTransient<ISerializer, ProtobufSerializer>();
        }
    }
}
