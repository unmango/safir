using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
// ReSharper disable once IdentifierTypo
public static class TestcontainersBuilderExtensions
{
    public static ITestcontainersBuilder<T> WithTestOutputHelper<T>(
        this ITestcontainersBuilder<T> builder,
        ITestOutputHelper outputHelper)
        where T : ITestcontainersContainer
        => builder.WithOutputConsumer(new TestOutputHelperOutputConsumer(outputHelper));
}
