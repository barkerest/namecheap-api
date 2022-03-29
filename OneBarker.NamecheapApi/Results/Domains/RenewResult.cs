using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:Renew command.
/// </summary>
public class RenewResult : IXmlParseableWithElementName
{
    /// <summary>
    /// Registered domain name.
    /// </summary>
    public string  DomainName    { get; private set; } = "";
    
    /// <summary>
    /// Unique integer value that represents the domain.
    /// </summary>
    public long     DomainID      { get; private set; }
    
    /// <summary>
    /// Indicates whether the domain was renewed successfully.
    /// </summary>
    public bool    Success       { get; private set; }
    
    /// <summary>
    /// Total amount charged for the renewal.
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
    
    /// <summary>
    /// Expiration date.
    /// </summary>
    public DateTime ExpireDate { get; private set; }
    
    /// <summary>
    /// Registration length in years.
    /// </summary>
    public int NumberOfYears { get; private set; }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        DomainName    = element.GetAttribute("DomainName");
        DomainID      = element.GetAttributeAsInt64("DomainID");
        Success       = element.GetAttributeAsBoolean("Renew");
        ChargedAmount = element.GetAttributeAsDecimal("ChargedAmount");
        OrderID       = element.GetAttributeAsInt64("OrderID");
        TransactionID = element.GetAttributeAsInt64("TransactionID");
        if (element.GetChild("DomainDetails") is { } domDet)
        {
            ExpireDate    = domDet.GetChildContentAsDateTime("ExpiredDate");
            NumberOfYears = domDet.GetChildContentAsInt32("NumYears");
        }
    }

    string IXmlParseableWithElementName.ElementName => "DomainRenewResult";
}
