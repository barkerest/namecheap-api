using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace OneBarker.NamecheapApi;

/// <summary>
/// A basic API configuration.
/// </summary>
public class ApiConfig : IApiConfig
{
    /// <summary>
    /// Create an API configuration.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="apiUser"></param>
    /// <param name="apiKey"></param>
    /// <param name="clientIp"></param>
    /// <param name="loggerFactory"></param>
    public ApiConfig(string host, string apiUser, string apiKey, IPAddress clientIp, ILoggerFactory? loggerFactory = null)
        : this(host, apiUser, apiKey, clientIp.ToString(), loggerFactory)
    {
        
    }

    /// <summary>
    /// Create an API configuration.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="apiUser"></param>
    /// <param name="apiKey"></param>
    /// <param name="clientIp"></param>
    /// <param name="loggerFactory"></param>
    public ApiConfig(string host, string apiUser, string apiKey, string clientIp, ILoggerFactory? loggerFactory = null)
    {
        Host          = host;
        ApiUser       = apiUser;
        UserName      = apiUser;
        ApiKey        = apiKey;
        ClientIp      = clientIp;
        LoggerFactory = loggerFactory ?? new NullLoggerFactory();
        ApiUri        = $"https://{Host}/xml.response";
    }

    /// <inheritdoc />
    public string Host { get; }

    /// <inheritdoc />
    public string ApiUri { get; }

    /// <inheritdoc />
    public string ApiUser { get; }

    /// <inheritdoc />
    public string ApiKey  { get; }

    /// <inheritdoc />
    public string UserName { get; }

    /// <inheritdoc />
    public string ClientIp { get; }

    /// <inheritdoc />
    public ILoggerFactory LoggerFactory { get; }
}
