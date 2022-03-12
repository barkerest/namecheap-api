namespace OneBarker.NamecheapApi;

/// <summary>
/// The configuration for an API request.
/// </summary>
public interface IApiRequest : IApiConfig
{
    /// <summary>
    /// The command to execute.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// Get the additional parameters for the request.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters();
}
