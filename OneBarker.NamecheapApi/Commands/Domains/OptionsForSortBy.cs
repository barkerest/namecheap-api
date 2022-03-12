namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Sort order for returned domain list.
/// </summary>
public enum OptionsForSortBy
{
    /// <summary>
    /// Sort by domain name in ascending order.
    /// </summary>
    Name,
    
    /// <summary>
    /// Sort by domain name in descending order.
    /// </summary>
    Name_Desc,
    
    /// <summary>
    /// Sort by expiration date in ascending order.
    /// </summary>
    ExpireDate,
    
    /// <summary>
    /// Sort by expiration date in descending order.
    /// </summary>
    ExpireDate_Desc,
    
    /// <summary>
    /// Sort by creation date in ascending order.
    /// </summary>
    CreateDate,
    
    /// <summary>
    /// Sort by creation date in descending order.
    /// </summary>
    CreateDate_Desc
}
