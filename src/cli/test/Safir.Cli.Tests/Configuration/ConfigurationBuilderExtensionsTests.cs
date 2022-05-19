using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Configuration;

public class ConfigurationBuilderExtensionsTests
{
    private readonly IConfigurationBuilder _builder = new ConfigurationBuilder();

    [Fact]
    public void AddDefaultUserProfileDirectory_AddsDirectoryEndingInSafir()
    {
        var configuration = _builder.AddDefaultUserProfileDirectory().Build();

        var result = configuration[SafirDefaults.ConfigDirectoryKey];

        Assert.EndsWith("safir", result);
    }

    // This test is context dependant due to usage of the static Environment class
    // and AddJsonFile(). If a valid json file exists in the user's ApplicationData
    // directory with keys that overlap SafirDefaults.ConfigDirectoryKey then this
    // test may fail. This should probably be fixed in the future, but it's a niche
    // edge case that can probably be ignored for now.
    [Fact]
    public void AddSafirCliDefault_AddsDirectoryEndingInSafir()
    {
        var configuration = _builder.AddSafirCliDefault().Build();

        var result = configuration[SafirDefaults.ConfigDirectoryKey];

        Assert.EndsWith("safir", result);
    }

    [Fact]
    public void AddSafirCliDefault_AddsConfigurationFile()
    {
        var builder = _builder.AddSafirCliDefault();

        var source = Assert.Single(builder.Sources, x => x.GetType() == typeof(JsonConfigurationSource));
        var jsonSource = Assert.IsType<JsonConfigurationSource>(source);
        Assert.EndsWith("config.json", jsonSource.Path);
        Assert.True(jsonSource.ReloadOnChange);
        Assert.True(jsonSource.Optional);
    }

    [Fact]
    public void AddSafirCliDefault_AddsRootConfiguration()
    {
        const string testVariableName = "SAFIR_SUPER_SECRET_TEST_VARIABLE";
        const string expectedTestValue = "test";
        Environment.SetEnvironmentVariable(testVariableName, expectedTestValue);

        var builder = _builder.AddSafirCliDefault();

        var source = Assert.Single(builder.Sources, x => x.GetType() == typeof(ChainedConfigurationSource));
        var root = Assert.IsType<ChainedConfigurationSource>(source).Configuration;

        var testValue = root[testVariableName];
        Assert.Equal(expectedTestValue, testValue);
    }
}
