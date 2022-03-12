using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace OneBarker.NamecheapApi.Tests;

public static class Config
{
    private static HttpClient     WebClient       { get; }
    public static  IPAddress      PublicIpAddress { get; }
    public static  IConfiguration Configuration   { get; }
    public static  IApiConfig     ApiConfig       { get; }
    
    static Config()
    {
        var cfgBuilder = new ConfigurationBuilder();
        var files = new[]
            {
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            }.Where(x => !string.IsNullOrWhiteSpace(x))
             .Select(x => x.Replace('\\', '/').TrimEnd('/') + "/OneBarker/test-config.json")
             .Distinct()
             .Append(Environment.CurrentDirectory.Replace('\\','/').TrimEnd('/') + "/test-config.json")
             .ToArray();
        
        foreach (var file in files)
        {
            cfgBuilder.AddJsonFile(file, true, false);
        }

        cfgBuilder.AddEnvironmentVariables("ONEB_");
        Configuration = cfgBuilder.Build();

        WebClient = new HttpClient();
        var knownIpEchoServices = new[]
        {
            "http://checkip.amazonaws.com/",
            "http://icanhazip.com",
        };

        var foundPubIp = false;
        foreach (var ipCheckUrl in knownIpEchoServices)
        {
            try
            {
                var pubIp = WebClient.GetStringAsync(ipCheckUrl).Result.Trim();
                PublicIpAddress = IPAddress.Parse(pubIp);
                foundPubIp      = true;
                break;
            }
            catch (Exception e) when (e is HttpRequestException or FormatException)
            {
                // ignored.
            }
        }

        if (!foundPubIp || PublicIpAddress is null)
            throw new InvalidOperationException("Failed to locate the public IP address of this system.");

        ApiConfig = new ApiConfig(
            KnownHost.Sandbox,
            Configuration["NamecheapApi:ApiUser"],
            Configuration["NamecheapApi:ApiKey"],
            PublicIpAddress
        );
    }
    
}
