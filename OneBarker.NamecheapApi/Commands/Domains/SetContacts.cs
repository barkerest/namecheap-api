using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Sets the contacts for a registered domain name.
/// </summary>
public class SetContacts : CommandBase, IApiCommandWithSingleResult<SetContactsResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public SetContacts(IApiConfig config)
        : base(config, "namecheap.domains.setContacts")
    {
    }

    /// <summary>
    /// The registered domain name.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; } = "";
    
    /// <summary>
    /// The registrant for the domain.
    /// </summary>
    public Contact Registrant { get; } = new();

    /// <summary>
    /// The technical contact for the domain.
    /// </summary>
    public Contact Tech { get; } = new();

    /// <summary>
    /// The administrative contact for the domain.
    /// </summary>
    public Contact Admin { get; } = new();

    /// <summary>
    /// The auxiliary billing contact for the domain.
    /// </summary>
    public Contact AuxBilling { get; } = new();
    
    /// <summary>
    /// Extended attributes for certain domains.
    /// </summary>
    public ICreateExtendedAttributes? ExtendedAttributes { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
        foreach (var v in ((ICommandParam)Registrant).GenerateParameters("Registrant"))
        {
            yield return v;
        }
        foreach (var v in ((ICommandParam)Tech).GenerateParameters("Tech"))
        {
            yield return v;
        }
        foreach (var v in ((ICommandParam)Admin).GenerateParameters("Admin"))
        {
            yield return v;
        }
        foreach (var v in ((ICommandParam)AuxBilling).GenerateParameters("AuxBilling"))
        {
            yield return v;
        }
        
        if (ExtendedAttributes is not null)
        {
            foreach (var v in ExtendedAttributes.GenerateParameters(""))
            {
                yield return v;
            }
        }
    }
}
