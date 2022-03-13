namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// Possible values for the WhoisGuard status.
/// </summary>
public enum OptionsForWhoisGuard
{
    /// <summary>
    /// WhoisGuard is enabled.
    /// </summary>
    Enabled,
    
    /// <summary>
    /// WhoisGuard is not present.
    /// </summary>
    NotPresent,
    
    /// <summary>
    /// Unknown status.
    /// </summary>
    Unknown
}
