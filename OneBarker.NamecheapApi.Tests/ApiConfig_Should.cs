using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.Tests;

#pragma warning disable xUnit2013

public class ApiConfig_Should
{
    private readonly ITestOutputHelper _output;

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
    {
        _output = outputHelper;
    }

    [Fact]
    public void BeAvailableForTesting()
    {
        _output.WriteLine("This test covers the loaded config for testing.\n");
        _output.WriteLine("The host is the sandbox host for testing.");
        _output.WriteLine("The client IP is the public IP address of the current system.");
        _output.WriteLine("The user and key come from the NamecheapApi:ApiUser and NamecheapApi:ApiKey values in the configuration.");
        _output.WriteLine("These values can be set in the [LocalAppData]/OneBarker/test-config.json file.");
        _output.WriteLine("Alternatively you can set ONEB_NAMECHEAPAPI_APIUSER and ONEB_NAMECHEAPAPI_APIKEY environment variables.\n");

        Assert.NotNull(Config.ApiConfig);
        Assert.False(string.IsNullOrWhiteSpace(Config.ApiConfig.Host));
        Assert.Equal(KnownHost.Sandbox, Config.ApiConfig.Host);
        Assert.NotNull(Config.ApiConfig.ClientIp);

        _output.WriteLine($"Host:      {Config.ApiConfig.Host}");
        _output.WriteLine($"Client IP: {Config.ApiConfig.ClientIp}");

        Assert.False(string.IsNullOrWhiteSpace(Config.ApiConfig.ApiUser));
        _output.WriteLine($"User:      {Config.ApiConfig.ApiUser}");
        Assert.False(string.IsNullOrWhiteSpace(Config.ApiConfig.ApiKey));
        _output.WriteLine($"Key:       {Config.ApiConfig.ApiKey[..5]}..{Config.ApiConfig.ApiKey[^5..]}");
        _output.WriteLine("\nSUCCESS");
        _output.WriteLine("");

        _output.WriteLine("The configuration is set, you may also need to ensure that your public IP is whitelisted in the sandbox.");
    }

    [Fact]
    public void BeValidForTesting()
    {
        var valid = Config.ApiConfig.IsValid(out var errors);
        if (!valid)
        {
            _output.WriteLine("Errors found:");
            foreach (var error in errors)
            {
                _output.WriteLine("  " + error);
            }

            throw new ApplicationException("Configuration is not valid for testing.");
        }

        _output.WriteLine("The configuration is valid.");
    }

    [Theory]
    [InlineData("")]     // blank
    [InlineData("   ")]  // blank
    [InlineData(":")]    // generates invalid URI, cannot be resolved.
    [InlineData(":123")] // generates invalid URI, cannot be resolved.
    public void RejectInvalidHost(string host)
    {
        var testConfig = new ApiConfig(host, Config.ApiConfig.ApiUser, Config.ApiConfig.ApiKey, Config.ApiConfig.ClientIp);
        var valid      = testConfig.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(2, errors.Length);
        Assert.Contains(errors, x => x.StartsWith("Host"));
        Assert.Contains(errors, x => x.StartsWith("ApiUri"));
    }

    [Theory]
    [InlineData("")]                      // blank
    [InlineData("   ")]                   // blank
    [InlineData("a01234567890123456789")] // 21 chars
    public void RejectInvalidUserNames(string userName)
    {
        var testConfig = new ApiConfig(Config.ApiConfig.Host, userName, Config.ApiConfig.ApiKey, Config.ApiConfig.ClientIp);
        var valid      = testConfig.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(2, errors.Length);
        Assert.Contains(errors, x => x.StartsWith("ApiUser"));
        Assert.Contains(errors, x => x.StartsWith("UserName"));
    }

    [Theory]
    [InlineData("")]                                                    // blank
    [InlineData("   ")]                                                 // blank
    [InlineData("a123456789b123456789c123456789d123456789e123456789f")] // 51 chars
    public void RejectInvalidApiKeys(string apiKey)
    {
        var testConfig = new ApiConfig(Config.ApiConfig.Host, Config.ApiConfig.ApiUser, apiKey, Config.ApiConfig.ClientIp);
        var valid      = testConfig.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.StartsWith("ApiKey"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("localhost")]
    [InlineData("100.200.300.400")]
    [InlineData("::1")]
    [InlineData("1:2:3:4::5")]
    public void RejectInvalidClientIp(string clientIp)
    {
        var testConfig = new ApiConfig(Config.ApiConfig.Host, Config.ApiConfig.ApiUser, Config.ApiConfig.ApiKey, clientIp);
        var valid      = testConfig.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.StartsWith("ClientIP"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void RejectInvalidCommands(string command)
    {
        var testConfig = new TestCommand(Config.ApiConfig, command);
        var valid      = testConfig.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.StartsWith("Command"));
    }
}
