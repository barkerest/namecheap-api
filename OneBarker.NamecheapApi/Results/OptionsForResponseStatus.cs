namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// Possible values for the response status.
/// </summary>
public enum OptionsForResponseStatus
{
    /// <summary>
    /// The command was successful.
    /// </summary>
    Ok,
    
    /// <summary>
    /// The command encountered one or more errors.
    /// </summary>
    Error,
    
    /// <summary>
    /// Unknown response type.
    /// </summary>
    Unknown
}
