using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.TestConfig;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests;

#pragma warning disable xUnit2013

public class ApiConfig_Should : CommonTestBase
{
    private class TestCommand : IApiCommand
    {
        public TestCommand(IApiConfig config, string command)
        {
            Host          = config.Host;
            ApiUri        = config.ApiUri;
            ApiUser       = config.ApiUser;
            ApiKey        = config.ApiKey;
            UserName      = config.UserName;
            ClientIp      = config.ClientIp;
            LoggerFactory = config.LoggerFactory;
            Command       = command;
        }

        public string         Host          { get; }
        public string         ApiUri        { get; }
        public string         ApiUser       { get; }
        public string         ApiKey        { get; }
        public string         UserName      { get; }
        public string         ClientIp      { get; }
        public ILoggerFactory LoggerFactory { get; }
        public string         Command       { get; }

        public IEnumerable<KeyValuePair<string, string>> AdditionalParameters => Array.Empty<KeyValuePair<string, string>>();
    }


    public ApiConfig_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void BeAvailableForTesting()
    {
        OutputHelper.WriteLine("This test covers the loaded config for testing.\n");
        OutputHelper.WriteLine("The host is the sandbox host for testing.");
        OutputHelper.WriteLine("The client IP is the public IP address of the current system.");
        OutputHelper.WriteLine("The user and key come from the NamecheapApi:ApiUser and NamecheapApi:ApiKey values in the configuration.");
        OutputHelper.WriteLine("These values can be set in the [LocalAppData]/OneBarker/test-config.json file.");
        OutputHelper.WriteLine("Alternatively you can set ONEB_NAMECHEAPAPI__APIUSER and ONEB_NAMECHEAPAPI__APIKEY environment variables.\n");

        Assert.NotNull(GoodConfig);
        Assert.False(string.IsNullOrWhiteSpace(GoodConfig.Host));
        Assert.Equal(KnownHost.Sandbox, GoodConfig.Host);
        Assert.NotNull(GoodConfig.ClientIp);

        OutputHelper.WriteLine($"Host:      {GoodConfig.Host}");
        OutputHelper.WriteLine($"Client IP: {GoodConfig.ClientIp}");

        Assert.False(string.IsNullOrWhiteSpace(GoodConfig.ApiUser));
        OutputHelper.WriteLine($"User:      {GoodConfig.ApiUser}");
        Assert.False(string.IsNullOrWhiteSpace(GoodConfig.ApiKey));
        OutputHelper.WriteLine($"Key:       {GoodConfig.ApiKey[..5]}..{GoodConfig.ApiKey[^5..]}");
        OutputHelper.WriteLine("\nSUCCESS");
        OutputHelper.WriteLine("");

        OutputHelper.WriteLine("The configuration is set, you may also need to ensure that your public IP is whitelisted in the sandbox.");
    }

    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(GoodConfig);

    [Theory]
    [InlineData("")]     // blank
    [InlineData("   ")]  // blank
    [InlineData(":")]    // generates invalid URI, cannot be resolved.
    [InlineData(":123")] // generates invalid URI, cannot be resolved.
    public void RejectInvalidHost(string host)
        => TestInvalidOption(GoodConfig.WithAlteredValue(host: host), host, new[] { "Host", "ApiUri" });

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid21CharString)]
    public void RejectInvalidUserNames(string userName)
        => TestInvalidOption(GoodConfig.WithAlteredValue(apiUser: userName), userName, new[] { "ApiUser", "UserName" });

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid51CharString)]
    public void RejectInvalidApiKeys(string apiKey)
        => TestInvalidOption(GoodConfig.WithAlteredValue(apiKey: apiKey), apiKey, "ApiKey");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("localhost")]
    [InlineData("100.200.300.400")]
    [InlineData("::1")]
    [InlineData("1:2:3:4::5")]
    public void RejectInvalidClientIp(string clientIp)
        => TestInvalidOption(GoodConfig.WithAlteredValue(clientIp: clientIp), clientIp, "ClientIp");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid81CharString)]
    public void RejectInvalidCommands(string command)
        => TestInvalidOption(new TestCommand(GoodConfig, command), command, "Command");
}
