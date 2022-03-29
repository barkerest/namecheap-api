using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

public class GetInfo : CommandBase, IApiCommandWithSingleResult<GetInfoResult>
{
    public GetInfo(IApiConfig config)
        : base(config, "namecheap.domains.getInfo")
    {
    }

    /// <summary>
    /// Domain name for which domain information needs to be requested.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; }
    
    /// <summary>
    /// Hosted domain name for which domain information needs to be requested
    /// </summary>
    [StringLength(255)]
    public string HostName { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
        if (!string.IsNullOrWhiteSpace(HostName)) yield return new KeyValuePair<string, string>("HostName", HostName);
    }
}
