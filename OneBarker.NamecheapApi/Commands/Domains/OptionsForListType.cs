namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Type of domain list to return.
/// </summary>
public enum OptionsForListType
{
    /// <summary>
    /// All domains.
    /// </summary>
    All,
    
    /// <summary>
    /// Domains that are expiring soon.
    /// </summary>
    Expiring,
    
    /// <summary>
    /// Domains that have already expired.
    /// </summary>
    Expired
}
