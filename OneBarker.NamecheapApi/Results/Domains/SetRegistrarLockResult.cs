using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:SetRegistrarLock command.
/// </summary>
public class SetRegistrarLockResult : IXmlParseable
{
    /// <summary>
    /// The domain name.
    /// </summary>
    public string Domain { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether the registrar lock was set successfully.
    /// </summary>
    public bool Success { get; private set; }


    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        element = element.GetChild("DomainSetRegistrarLockResult") ?? throw new InvalidOperationException("Missing result.");

        Domain  = element.GetAttribute("Domain");
        Success = element.GetAttributeAsBoolean("IsSuccess");

    }
}
