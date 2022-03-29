using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:SetContacts command.
/// </summary>
public class SetContactsResult : IXmlParseableWithElementName
{
    /// <summary>
    /// The registered domain name.
    /// </summary>
    public string Domain { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether contact details were set successfully.
    /// </summary>
    public bool Success { get; private set; }
    
    
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Domain  = element.GetAttribute("Domain");
        Success = element.GetAttributeAsBoolean("IsSuccess");
    }

    string IXmlParseableWithElementName.ElementName => "DomainSetContactResult";
}
