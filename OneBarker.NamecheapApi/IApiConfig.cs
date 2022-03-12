namespace OneBarker.NamecheapApi;

/// <summary>
/// The base configuration for API requests.
/// </summary>
public interface IApiConfig
{
    /// <summary>
    /// The name of the host for the API.
    /// </summary>
    /// <remarks>
    /// This will generally be 'api.namecheap.com' or 'api.sandbox.namecheap.com'.
    /// Use the sandbox when testing and developing.
    /// </remarks>
    public string Host { get; }
    
    /// <summary>
    /// The full URI for the API.
    /// </summary>
    /// <remarks>
    /// This will generally be 'https://{HOST}/xml.response'.
    /// </remarks>
    public string ApiUri { get; }
    
    /// <summary>
    /// The API authentication user.
    /// </summary>
    public string ApiUser { get; }
    
    /// <summary>
    /// The API authentication key.
    /// </summary>
    public string ApiKey { get; }
    
    /// <summary>
    /// The user executing the API actions.
    /// </summary>
    /// <remarks>
    /// This will generally be the same as the ApiUser.
    /// </remarks>
    public string UserName { get; }
    
    /// <summary>
    /// The client IP address (must be an IPv4 address).
    /// </summary>
    /// <remarks>
    /// This should normally be your public IP address.
    /// </remarks>
    public string ClientIp { get; }
}

