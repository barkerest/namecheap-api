using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains.Dns;

/// <summary>
/// The result from the Domains:Dns:SetHosts command.
/// </summary>
public class SetHostsResult : IXmlParseableWithElementName
{
    /// <summary>
    /// The domain name for which you are trying to set host records.
    /// </summary>
    public string Domain { get; private set; }
    
    /// <summary>
    /// Indicates whether host records were set successfully.
    /// </summary>
    public bool Success { get; private set; }
    
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Domain  = element.GetAttribute("Domain");
        Success = element.GetAttributeAsBoolean("IsSuccess");
    }

    string IXmlParseableWithElementName.ElementName => "DomainDNSSetHostsResult";
}
