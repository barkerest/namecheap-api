using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from a domain create command.
/// </summary>
public class CreateResult : IXmlParseableWithElementName
{
    /// <summary>
    /// Domain named being registered.
    /// </summary>
    public string DomainName { get; private set; } = "";
    
    /// <summary>
    /// Indicates if the domain was registered.
    /// </summary>
    public bool Registered { get; private set; }
    
    /// <summary>
    /// Total amount charged for registration.
    /// </summary>
    public decimal ChargedAmount { get; private set; }
    
    /// <summary>
    /// Unique integer ID for the domain.
    /// </summary>
    public int DomainID { get; private set; }
    
    /// <summary>
    /// Unique integer ID for the order.
    /// </summary>
    public long OrderID { get; private set; }
    
    /// <summary>
    /// Unique integer ID for the transaction.
    /// </summary>
    public long TransactionID { get; private set; }
    
    /// <summary>
    /// Indicates whether domain private protection is enabled for the domain.
    /// </summary>
    public bool WhoisGuardEnabled { get; private set; }
    
    /// <summary>
    /// Indicates if the domain is instant (real-time).
    /// </summary>
    public bool NonRealTimeDomain { get; private set; }
    
    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        DomainName = element.GetAttribute("Domain") 
                 ?? throw new InvalidOperationException("Missing domain attribute in create response.");

        Registered        = element.GetAttributeAsBoolean("Registered");
        ChargedAmount     = element.GetAttributeAsDecimal("ChargedAmount");
        DomainID          = element.GetAttributeAsInt32("DomainID");
        OrderID           = element.GetAttributeAsInt64("OrderID");
        TransactionID     = element.GetAttributeAsInt64("TransactionID");
        WhoisGuardEnabled = element.GetAttributeAsBoolean("WhoisguardEnable");
        NonRealTimeDomain = element.GetAttributeAsBoolean("NonRealTimeDomain");
    }

    /// <inheritdoc />
    string IXmlParseableWithElementName.ElementName
        => "DomainCreateResult";
}
