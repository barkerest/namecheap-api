using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Reactivates an expired domain.
/// </summary>
public class Reactivate : CommandBase, IApiCommandWithSingleResult<ReactivateResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public Reactivate(IApiConfig config)
        : base(config, "namecheap.domains.reactivate")
    {
    }

    /// <summary>
    /// Domain name to reactivate.
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName { get; set; } = "";
    
    /// <summary>
    /// Number of years to add.
    /// </summary>
    public int Years { get; set; } = 2;

    /// <summary>
    /// Promotional code for renewing the domain.
    /// </summary>
    [StringLength(20)]
    public string   PromotionCode   { get; set; } = "";
    
    /// <summary>
    /// Indication if the domain is premium.
    /// </summary>
    public bool?    IsPremiumDomain { get; set; }
    
    /// <summary>
    /// Renewal price for the premium domain.
    /// </summary>
    public decimal? PremiumPrice    { get; set; }

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainName", DomainName);
        yield return new KeyValuePair<string, string>("YearsToAdd", Years.ToString());
        if (!string.IsNullOrWhiteSpace(PromotionCode)) yield return new KeyValuePair<string, string>("PromotionCode", PromotionCode);
        if (IsPremiumDomain.HasValue)
        {
            yield return new KeyValuePair<string, string>("IsPremiumDomain", IsPremiumDomain.GetValueOrDefault().ToString());
            yield return new KeyValuePair<string, string>("PremiumPrice", PremiumPrice.GetValueOrDefault().ToString("0.00"));
        }
    }
}
