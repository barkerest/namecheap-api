using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Gets the Registrar Lock status for the requested domain.
/// </summary>
public class GetRegistrarLock : CommandBase, IApiCommand<GetRegistrarLockResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public GetRegistrarLock(IApiConfig config)
        : base(config, "namecheap.domains.getRegistrarLock")
    {
        
    }

    /// <summary>
    /// Domain name to get status for.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; } = "";

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
    }
}
