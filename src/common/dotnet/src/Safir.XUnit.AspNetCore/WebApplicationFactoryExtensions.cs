using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Safir.XUnit.AspNetCore;

[PublicAPI]
public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<T> WithLogger<T>(this WebApplicationFactory<T> factory, ITestOutputHelper outputHelper)
        where T : class
        => factory.WithWebHostBuilder(builder => {
            builder.ConfigureLogging(logging => logging.AddXunitLogging(outputHelper));
        });

    public static WebApplicationFactory<T> WithLogger<T>(this WebApplicationFactory<T> factory, IMessageSink sink)
        where T : class
        => factory.WithWebHostBuilder(builder => {
            builder.ConfigureLogging(logging => logging.AddXunitLogging(sink));
        });
}
