using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains.Dns;

namespace OneBarker.NamecheapApi.Commands.Domains.Dns;

/// <summary>
/// Retrieves DNS host record settings for the requested domain.
/// </summary>
public class GetHosts : CommandBase, IApiCommandWithSingleResult<GetHostsResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public GetHosts(IApiConfig config)
        : base(config, "namecheap.domains.dns.getHosts")
    {
    }

    /// <summary>
    /// SLD of the domain to setHosts.
    /// </summary>
    [Required, StringLength(70)]
    public string SLD { get; set; } = "";

    /// <summary>
    /// TLD of the domain to setHosts.
    /// </summary>
    [Required, StringLength(10)]
    public string TLD { get; set; } = "";

    /// <summary>
    /// Domain name of the domain to setHosts (SLD + '.' + TLD).
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName
    {
        get => (string.IsNullOrWhiteSpace(SLD) && string.IsNullOrWhiteSpace(TLD)) ? "" : (SLD + '.' + TLD);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SLD = TLD = "";
            }
            else
            {
                var chunks = value.Split('.', 2);
                SLD = chunks[0];
                TLD = chunks.Length < 2 ? "" : chunks[1];
            }
        }
    }

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("SLD", SLD);
        yield return new KeyValuePair<string, string>("TLD", TLD);
    }
}
