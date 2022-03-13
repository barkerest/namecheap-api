using OneBarker.NamecheapApi.Results;

namespace OneBarker.NamecheapApi;

/// <summary>
/// A response from the API.
/// </summary>
public interface IApiResponse : IXmlParseable
{
    /// <summary>
    /// The response status.
    /// </summary>
    public OptionsForResponseStatus Status { get; }

    /// <summary>
    /// Any errors encountered.
    /// </summary>
    public IReadOnlyList<ErrorMessage> Errors { get; }
    
    /// <summary>
    /// The command that was requested.
    /// </summary>
    public string RequestedCommand { get; }
    
    /// <summary>
    /// The server that executed the command.
    /// </summary>
    public string Server { get; }
    
    /// <summary>
    /// The offset from GMT time for the server.
    /// </summary>
    public string GmtTimeDifference { get; }
    
    /// <summary>
    /// The command execution time in seconds.
    /// </summary>
    public double ExecutionTime { get; } 
}

/// <summary>
/// A response from the API with a payload.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IApiResponse<T> : IApiResponse where T : class, IXmlParseable, new()
{
    /// <summary>
    /// The specific response from the command.
    /// </summary>
    public T CommandResponse { get; }
}

