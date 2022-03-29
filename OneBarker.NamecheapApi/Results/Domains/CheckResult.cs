using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// A result for a single domain from a Domains:Check command.
/// </summary>
public class CheckResult : IXmlParseableWithElementName
{
    /// <summary>
    /// Domain name for which you wish to check availability.
    /// </summary>
    public string Domain { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether the domain name is available for registration
    /// </summary>
    public bool Available { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain name is premium
    /// </summary>
    public bool IsPremium { get; private set; }
    
    /// <summary>
    /// Registration Price for the premium domain
    /// </summary>
    public decimal PremiumRegistrationPrice { get; private set; }
    
    /// <summary>
    /// Renewal price for the premium domain
    /// </summary>
    public decimal PremiumRenewalPrice { get; private set; }
    
    /// <summary>
    /// Restore price for the premium domain
    /// </summary>
    public decimal PremiumRestorePrice { get; private set; }
    
    /// <summary>
    /// Transfer price for the premium domain
    /// </summary>
    public decimal PremiumTransferPrice { get; private set; }
    
    /// <summary>
    /// Fee charged by ICANN
    /// </summary>
    public decimal IcannFee { get; private set; }
    
    /// <summary>
    /// Purchase fee for the premium domain during Early Access Program (EAP)
    /// </summary>
    public decimal EapFee { get; private set; }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Domain                   = element.GetAttribute("Domain");
        Available                = element.GetAttributeAsBoolean("Available");
        IsPremium                = element.GetAttributeAsBoolean("IsPremiumName");
        PremiumRegistrationPrice = element.GetAttributeAsDecimal("PremiumRegistrationPrice");
        PremiumRenewalPrice      = element.GetAttributeAsDecimal("PremiumRenewalPrice");
        PremiumRestorePrice      = element.GetAttributeAsDecimal("PremiumRestorePrice");
        PremiumTransferPrice     = element.GetAttributeAsDecimal("PremiumTransferPrice");
        IcannFee                 = element.GetAttributeAsDecimal("IcannFee");
        EapFee                   = element.GetAttributeAsDecimal("EapFee");
    }

    string IXmlParseableWithElementName.ElementName => "DomainCheckResult";
}
