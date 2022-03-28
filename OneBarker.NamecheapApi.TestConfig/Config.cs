using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OneBarker.NamecheapApi.TestConfig;

public static class Config
{
    private class ApiConfigWrapper : IApiConfig
    {
        public ApiConfigWrapper(IApiConfig config, Action<ILoggingBuilder> configLogging)
        {
            Host          = config.Host;
            ApiUri        = config.ApiUri;
            ApiUser       = config.ApiUser;
            ApiKey        = config.ApiKey;
            UserName      = config.UserName;
            ClientIp      = config.ClientIp;
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(configLogging);
        }

        public ApiConfigWrapper(IApiConfig config, string? host = null, string? apiUri = null, string? apiUser = null, string? apiKey = null, string? userName = null, string? clientIp = null)
        {
            Host          = host ?? config.Host;
            ApiUri        = apiUri ?? $"https://{Host}/xml.response";
            ApiUser       = apiUser ?? userName ?? config.ApiUser;
            ApiKey        = apiKey ?? config.ApiKey;
            UserName      = userName ?? apiUser ?? config.UserName;
            ClientIp      = clientIp ?? config.ClientIp;
            LoggerFactory = config.LoggerFactory;
        }
        
        public string         Host          { get; }
        public string         ApiUri        { get; }
        public string         ApiUser       { get; }
        public string         ApiKey        { get; }
        public string         UserName      { get; }
        public string         ClientIp      { get; }
        public ILoggerFactory LoggerFactory { get; }
    }
    
    private static HttpClient     WebClient       { get; }
    public static  IPAddress      PublicIpAddress { get; }
    public static  IConfiguration Configuration   { get; }
    public static  IApiConfig     ApiConfig       { get; }

    public static IApiConfig ApiConfigWithLogging(Action<ILoggingBuilder> configLogging)
        => new ApiConfigWrapper(ApiConfig, configLogging);

    public static IApiConfig WithAlteredValue(this IApiConfig self, string? host = null, string? apiUser = null, string? apiKey = null, string? clientIp = null)
        => new ApiConfigWrapper(self, host, null, apiUser, apiKey, apiUser, clientIp);
    
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
        cfgBuilder.AddCommandLine(
            Environment.GetCommandLineArgs(),
            new Dictionary<string, string>()
            {
                { "--api-user", "NamecheapApi:ApiUser" },
                { "--api-key", "NamecheapApi:ApiKey" },
                { "--test-domain-name", "NamecheapApi:TestDomainName" },
                { "--test-domain-tld", "NamecheapApi:TestDomainTLD" },
                { "--test-domain", "NamecheapApi:ExplicitTestDomain" },
            }
        );
        
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

    public static bool BasicLoggingFilter(string category, LogLevel level)
    {
        if (level >= LogLevel.Error) return true;

        category = category.ToLower();
        if (category.StartsWith("microsoft.") ||
            category.StartsWith("system.")) return level >= LogLevel.Warning;

        return true;
    }
}
