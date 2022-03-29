using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Gets the contacts for a registered domain name.
/// </summary>
public class GetContacts : CommandBase, IApiCommandWithSingleResult<GetContactsResult>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    public GetContacts(IApiConfig config)
        : base(config, "namecheap.domains.getContacts")
    {
    }

    /// <summary>
    /// The registered domain name.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; } = "";
    
    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
    }
}
