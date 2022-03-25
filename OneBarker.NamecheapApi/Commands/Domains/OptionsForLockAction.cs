namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// The actions available for locking a domain.
/// </summary>
public enum OptionsForLockAction
{
    /// <summary>
    /// Set the registrar lock.
    /// </summary>
    Lock,
    
    /// <summary>
    /// Remove the registrar lock.
    /// </summary>
    Unlock
}
