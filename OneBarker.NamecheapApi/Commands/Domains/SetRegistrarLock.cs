using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Sets the Registrar Lock status for a domain.
/// </summary>
public class SetRegistrarLock : CommandBase, IApiCommandWithSingleResult<SetRegistrarLockResult>
{
    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public SetRegistrarLock(IApiConfig config)
        : base(config, "namecheap.domains.setRegistrarLock")
    {
    }

    /// <summary>
    /// Domain name to set status for.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; } = "";

    /// <summary>
    /// The action to perform.
    /// </summary>
    public OptionsForLockAction LockAction { get; set; } = OptionsForLockAction.Lock;

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
        yield return new KeyValuePair<string, string>("LockAction", LockAction.ToString().ToUpper());
    }
}
