using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Safir.FileManager.Service.Services
{
    internal static class EndpointRouteBuilderExtensions
    {
        public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGrpcService<TrackerService>();
        }
    }
}
