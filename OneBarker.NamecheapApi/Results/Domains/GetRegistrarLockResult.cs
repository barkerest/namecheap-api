using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:GetRegistrarLock command.
/// </summary>
public class GetRegistrarLockResult : IXmlParseable
{
    /// <summary>
    /// The domain name.
    /// </summary>
    public string Domain { get; private set; } = "";
    
    /// <summary>
    /// The lock status.
    /// </summary>
    public bool RegistrarLockStatus { get; private set; }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        element             = element.GetChild("DomainGetRegistrarLockResult") ?? throw new InvalidOperationException("Missing result.");
        
        Domain              = element.GetAttribute("Domain");
        RegistrarLockStatus = element.GetAttributeAsBoolean("RegistrarLockStatus");
    }
}
