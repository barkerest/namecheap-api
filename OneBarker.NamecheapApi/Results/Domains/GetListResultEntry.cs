using System.Net;
using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

public class GetListResultEntry : IXmlParseable
{
    /// <summary>
    /// Integer identity for this domain.
    /// </summary>
    public int                  ID         { get; private set; }
    
    /// <summary>
    /// The registered domain name.
    /// </summary>
    public string               Name       { get; private set; } = "";
    
    /// <summary>
    /// User account under which the domain is registered.
    /// </summary>
    public string               User       { get; private set; } = "";
    
    /// <summary>
    /// Domain creation date.
    /// </summary>
    public DateTime             Created    { get; private set; }
    
    /// <summary>
    /// Domain expiration date.
    /// </summary>
    public DateTime             Expires    { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain is expired.
    /// </summary>
    public bool                 IsExpired  { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain is locked.
    /// </summary>
    public bool                 IsLocked   { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain is set for auto-renew.
    /// </summary>
    public bool                 AutoRenew  { get; private set; }
    
    /// <summary>
    /// The domain privacy status.
    /// </summary>
    public OptionsForWhoisGuard WhoisGuard { get; private set; } = OptionsForWhoisGuard.Unknown;
    
    /// <summary>
    /// Indicates whether the domain is premium.
    /// </summary>
    public bool                 IsPremium  { get; private set; }
    
    /// <summary>
    /// Indicates whether the DNS is hosted with Namecheap.
    /// </summary>
    public bool                 IsOurDns   { get; private set; }
    
    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        ID         = element.GetAttributeAsInt32("ID");
        Name       = element.GetAttribute("Name") ?? throw new InvalidOperationException("Name is required for domain entries.");
        User       = element.GetAttribute("User") ?? "";
        Created    = element.GetAttributeAsDateTime("Created");
        Expires    = element.GetAttributeAsDateTime("Expires");
        IsExpired  = element.GetAttributeAsBoolean("IsExpired");
        IsLocked   = element.GetAttributeAsBoolean("IsLocked");
        AutoRenew  = element.GetAttributeAsBoolean("AutoRenew");
        WhoisGuard = element.GetAttributeAsEnum("WhoisGuard", OptionsForWhoisGuard.Unknown);
        IsPremium  = element.GetAttributeAsBoolean("IsPremium");
        IsOurDns   = element.GetAttributeAsBoolean("IsOurDNS");
    }
}
