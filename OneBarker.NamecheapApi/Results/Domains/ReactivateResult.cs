using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:Reactivate command.
/// </summary>
public class ReactivateResult : IXmlParseableWithElementName
{
    /// <summary>
    /// The registered domain name.
    /// </summary>
    public string  Domain        { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether the domain was reactivated successfully.
    /// </summary>
    public bool    Success       { get; private set; }
    
    /// <summary>
    /// Total amount charged for reactivation.
    /// </summary>
    public decimal ChargedAmount { get; private set; }
    
    /// <summary>
    /// Unique integer value that represents the order.
    /// </summary>
    public long    OrderID       { get; private set; }
    
    /// <summary>
    /// Unique integer value that represents the transaction.
    /// </summary>
    public long    TransactionID { get; private set; }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Domain        = element.GetAttribute("Domain");
        Success       = element.GetAttributeAsBoolean("IsSuccess");
        ChargedAmount = element.GetAttributeAsDecimal("ChargedAmount");
        OrderID       = element.GetAttributeAsInt64("OrderID");
        TransactionID = element.GetAttributeAsInt64("TransactionID");
    }

    string IXmlParseableWithElementName.ElementName => "DomainReactivateResult";
}
