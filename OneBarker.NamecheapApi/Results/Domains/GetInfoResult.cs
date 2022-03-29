using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// Result from the Domains:GetInfo command.
/// </summary>
public class GetInfoResult : IXmlParseableWithElementName
{
    /// <summary>
    /// Indicates the status of the domain.
    /// </summary>
    public OptionsForDomainStatus Status { get; private set; } = OptionsForDomainStatus.Ok;
    
    /// <summary>
    /// Unique integer value that represents the domain.
    /// </summary>
    public long DomainID { get; private set; }
    
    /// <summary>
    /// Domain for which information was requested.
    /// </summary>
    public string DomainName { get; private set; }
    
    
    /// <summary>
    /// The user account under which the domain is registered.
    /// </summary>
    public string OwnerName { get; private set; }
    
    /// <summary>
    /// Indicates whether the API user is the owner of the domain.
    /// </summary>
    public bool IsOwner { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain name is premium.
    /// </summary>
    public bool IsPremium { get; private set; }
    
    /// <summary>
    /// Date the domain was created under Namecheap.
    /// </summary>
    public DateTime CreatedDate { get; private set; }
    
    /// <summary>
    /// Date the domain expires.
    /// </summary>
    public DateTime ExpireDate { get; private set; }

    /// <summary>
    /// Domain lock details (raw XML since format not provided).
    /// </summary>
    public string LockDetails { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether domain privacy is enabled.
    /// </summary>
    public bool WhoisGuardEnabled { get; private set; }
    
    /// <summary>
    /// Unique numeric ID representing the domain privacy account.
    /// </summary>
    public long WhoisGuardID { get; private set; }
    
    /// <summary>
    /// Date that domain privacy expires.
    /// </summary>
    public DateTime WhoisGuardExpiration { get; private set; }

    /// <summary>
    /// DNS provider type.
    /// </summary>
    public string DnsProvider { get; private set; } = "";
    
    /// <summary>
    /// Details of DNS configuration (raw XML since format details not provided).
    /// </summary>
    public string DnsDetails { get; private set; }
    
    /// <summary>
    /// Indicates whether the API user has full modification rights for the domain.
    /// </summary>
    public bool HaveAllModificationRights { get; private set; }

    /// <summary>
    /// Modification rights for the API user (raw XML since format details not provided).
    /// </summary>
    public string ModificationRights { get; private set; } = "";
    
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Status     = element.GetAttributeAsEnum<OptionsForDomainStatus>("Status");
        DomainID   = element.GetAttributeAsInt32("ID");
        DomainName = element.GetAttribute("DomainName");
        OwnerName  = element.GetAttribute("OwnerName");
        IsOwner    = element.GetAttributeAsBoolean("IsOwner");
        IsPremium  = element.GetAttributeAsBoolean("IsPremium");
        if (element.GetChild("DomainDetails") is { } domDet)
        {
            CreatedDate = domDet.GetAttributeAsDateTime("CreatedDate");
            ExpireDate  = domDet.GetAttributeAsDateTime("ExpiredDate");
        }
        LockDetails = element.GetChild("LockDetails")?.OuterXml ?? "";
        if (element.GetChild("Whoisguard") is { } wg)
        {
            WhoisGuardEnabled    = wg.GetAttributeAsBoolean("Enabled");
            WhoisGuardID         = wg.GetChildContentAsInt32("ID");
            WhoisGuardExpiration = wg.GetChildContentAsDateTime("ExpiredDate");
        }

        if (element.GetChild("DnsDetails") is { } dns)
        {
            DnsProvider = dns.GetAttribute("ProviderType");
            DnsDetails  = dns.OuterXml;
        }

        if (element.GetChild("Modificationrights") is { } mods)
        {
            HaveAllModificationRights = mods.GetAttributeAsBoolean("All");
            ModificationRights        = mods.OuterXml;
        }
    }

    string IXmlParseableWithElementName.ElementName => "DomainGetInfoResult";
}
