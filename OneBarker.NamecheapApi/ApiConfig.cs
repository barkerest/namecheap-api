using System.Net;

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
    public ApiConfig(string host, string apiUser, string apiKey, IPAddress clientIp)
        : this(host, apiUser, apiKey, clientIp.ToString())
    {
        
    }

    /// <summary>
    /// Create an API configuration.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="apiUser"></param>
    /// <param name="apiKey"></param>
    /// <param name="clientIp"></param>
    public ApiConfig(string host, string apiUser, string apiKey, string clientIp)
        
    {
        Host     = host;
        ApiUser  = apiUser;
        UserName = apiUser;
        ApiKey   = apiKey;
        ClientIp = clientIp;
        ApiUri   = $"https://{Host}/xml.response";
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
}
