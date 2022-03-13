using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Models;

namespace OneBarker.NamecheapApi.Commands.Domains;

public class Create : CommandBase
{
    
    public Create(IApiConfig config)
        : base(config, "namecheap.domains.create")
    {
    }

    /// <summary>
    /// Domain name to register.
    /// </summary>
    public string DomainName { get; set; } = "";

    /// <summary>
    /// Number of years to register.
    /// </summary>
    public int YearsToRegister { get; set; } = 2;

    /// <summary>
    /// Promotion code for the domain.
    /// </summary>
    public string PromotionCode { get; set; } = "";

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
    /// The billing contact (not required, but AuxBilling is).
    /// </summary>
    public Contact? Billing { get; set; }

    /// <summary>
    /// The Internationalized Domain Name code.
    /// </summary>
    [StringLength(100)]
    public string IdnCode { get; set; } = "";

    /// <summary>
    /// Extended attributes for certain domains.
    /// </summary>
    public ICreateExtendedAttributes? ExtendedAttributes { get; set; }

    /// <summary>
    /// Custom nameservers to associate with the domain.
    /// </summary>
    public string[] Nameservers { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// Adds free domain privacy to the domain.
    /// </summary>
    public bool? AddFreeWhoisGuard { get; set; }
    
    /// <summary>
    /// Enabled free domain privacy for the domain.
    /// </summary>
    public bool? EnableWhoisGuard { get; set; }
    
    /// <summary>
    /// Indicate if the domain name is premium.
    /// </summary>
    public bool? IsPremiumDomain { get; set; }
    
    /// <summary>
    /// The registration price for the premium domain.
    /// </summary>
    public decimal? PremiumPrice { get; set; }
    
    /// <summary>
    /// The purchase fee for a premium domain during Early Access Program.
    /// </summary>
    public decimal? EapFee { get; set; }
    
    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
        yield return new KeyValuePair<string, string>("Years", YearsToRegister.ToString());
        if (!string.IsNullOrWhiteSpace(PromotionCode)) yield return new KeyValuePair<string, string>("PromotionCode", PromotionCode);
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

        if (Billing is not null)
        {
            foreach (var v in ((ICommandParam)Billing).GenerateParameters("Billing"))
            {
                yield return v;
            }
        }

        if (!string.IsNullOrWhiteSpace(IdnCode)) yield return new KeyValuePair<string, string>("IdnCode", IdnCode);
        
        if (ExtendedAttributes is not null)
        {
            foreach (var v in ExtendedAttributes.GenerateParameters(""))
            {
                yield return v;
            }
        }
        
        if (Nameservers.Any()) yield return new KeyValuePair<string, string>("Nameservers", string.Join(',', Nameservers));
        if (AddFreeWhoisGuard.HasValue) yield return new KeyValuePair<string, string>("AddFreeWhoisguard", AddFreeWhoisGuard.GetValueOrDefault() ? "yes" : "no");
        if (EnableWhoisGuard.HasValue) yield return new KeyValuePair<string, string>("WGEnabled", EnableWhoisGuard.GetValueOrDefault() ? "yes" : "no");
        if (IsPremiumDomain.HasValue) yield return new KeyValuePair<string, string>("IsPremiumDomain", IsPremiumDomain.GetValueOrDefault().ToString());
        if (PremiumPrice.HasValue) yield return new KeyValuePair<string, string>("PremiumPrice", PremiumPrice.GetValueOrDefault().ToString("0.00"));
        if (EapFee.HasValue) yield return new KeyValuePair<string, string>("EapFee", EapFee.GetValueOrDefault().ToString("0.00"));
    }
}
