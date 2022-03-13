namespace OneBarker.NamecheapApi;

/// <summary>
/// The configuration for an API command.
/// </summary>
public interface IApiCommand : IApiConfig
{
    /// <summary>
    /// The command to execute.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// The additional parameters for the command.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> AdditionalParameters { get; }
}

/// <summary>
/// The configuration for an API command with a strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCommand<TResponse> : IApiCommand where TResponse : class, IXmlParseable, new()
{
    
}

