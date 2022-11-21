using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Safir.XUnit.AspNetCore;

[PublicAPI]
public static class XUnitLoggerExtensions
{
    public static ILoggingBuilder AddXunitLogging(this ILoggingBuilder builder, ITestOutputHelper outputHelper)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider>(new TestOutputHelperLoggerProvider(outputHelper)));

        return builder;
    }

    public static ILoggingBuilder AddXunitLogging(this ILoggingBuilder builder, IMessageSink sink)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(new MessageSinkLoggerProvider(sink)));

        return builder;
    }
}
