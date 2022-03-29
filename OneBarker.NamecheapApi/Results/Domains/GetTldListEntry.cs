using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

public class GetTldListEntry : IXmlParseable
{
    /// <summary>
    /// Indicates the top-level domain.
    /// </summary>
    public string Name { get; private set; } = "";

    /// <summary>
    /// Indicates whether the domain registration is instant.
    /// </summary>
    public bool NonRealTime { get; private set; }

    /// <summary>
    /// Minimum number of years the TLD can be registered for.
    /// </summary>
    public int MinRegisterYears { get; private set; }

    /// <summary>
    /// Maximum number of years the TLD can be registered for.
    /// </summary>
    public int MaxRegisterYears { get; private set; }

    /// <summary>
    /// Minimum number of years the TLD can be renewed for.
    /// </summary>
    public int MinRenewYears { get; private set; }

    /// <summary>
    /// Maximum number of years the TLD can be renewed for.
    /// </summary>
    public int MaxRenewYears { get; private set; }

    /// <summary>
    /// Minimum number of years the TLD can be transferred for.
    /// </summary>
    public int MinTransferYears { get; private set; }

    /// <summary>
    /// Maximum number of years the TLD can be transferred for. 
    /// </summary>
    public int MaxTransferYears { get; private set; }

    /// <summary>
    /// Indicates whether a domain with this TLD can be registered through API.
    /// </summary>
    public bool IsApiRegisterable { get; private set; }

    /// <summary>
    /// Indicates whether a domain with this TLD can be renewed through API.
    /// </summary>
    public bool IsApiRenewable { get; private set; }

    /// <summary>
    /// Indicates whether a domain with this TLD can be transferred to Namecheap through API.
    /// </summary>
    public bool IsApiTransferable { get; private set; }

    /// <summary>
    /// Indicates whether EPP code is required for this TLD.
    /// </summary>
    public bool IsEppRequired { get; private set; }

    /// <summary>
    /// Indicates whether contact details can be modified for this TLD.
    /// </summary>
    public bool ContactModificationDisabled { get; private set; }

    /// <summary>
    /// Indicates whether domain privacy can be allotted for this TLD.
    /// </summary>
    public bool DomainPrivacyCanBeAllotted { get; private set; }

    /// <summary>
    /// Indicates whether this TLD is shown in general search results or in extended search results only.
    /// </summary>
    public bool IsIncludeInExtendedSearchOnly { get; private set; }

    /// <summary>
    /// Indicates the sorting order in which TLDs are displayed on Namecheap website’s domain search results page.
    /// </summary>
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// Indicates whether this is a generic TLD or country-code TLD.
    /// </summary>
    public string Type { get; private set; } = "";

    /// <summary>
    /// Indicates whether IDN is supported for this TLD.
    /// </summary>
    public bool SupportsIDN { get; private set; }

    /// <summary>
    /// Indicates the category of the domain.
    /// </summary>
    public string Category { get; private set; } = "";

    /// <summary>
    /// A short description.
    /// </summary>
    public string Description { get; private set; } = "";

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Name                          = element.GetAttribute("Name");
        NonRealTime                   = element.GetAttributeAsBoolean("NonRealTime");
        MinRegisterYears              = element.GetAttributeAsInt32("MinRegisterYears");
        MaxRegisterYears              = element.GetAttributeAsInt32("MaxRegisterYears");
        MinRenewYears                 = element.GetAttributeAsInt32("MinRenewYears");
        MaxRenewYears                 = element.GetAttributeAsInt32("MaxRenewYears");
        MinTransferYears              = element.GetAttributeAsInt32("MinTransferYears");
        MaxTransferYears              = element.GetAttributeAsInt32("MaxTransferYears");
        IsApiRegisterable             = element.GetAttributeAsBoolean("IsApiRegisterable");
        IsApiRenewable                = element.GetAttributeAsBoolean("IsApiRenewable");
        IsApiTransferable             = element.GetAttributeAsBoolean("IsApiTransferable");
        IsEppRequired                 = element.GetAttributeAsBoolean("IsEppRequired");
        ContactModificationDisabled   = element.GetAttributeAsBoolean("IsDisableModContact");
        DomainPrivacyCanBeAllotted    = element.GetAttributeAsBoolean("IsDisableWGAllot");
        IsIncludeInExtendedSearchOnly = element.GetAttributeAsBoolean("IsIncludedInExtendedSearchOnly");
        SequenceNumber                = element.GetAttributeAsInt32("SequenceNumber");
        Type                          = element.GetAttribute("Type");
        SupportsIDN                   = element.GetAttributeAsBoolean("IsSupportsIDN");
        Category                      = element.GetAttribute("Category");
        Description                   = element.GetContent();
    }
}
