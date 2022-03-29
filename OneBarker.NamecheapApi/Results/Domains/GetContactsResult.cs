using System.Xml;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:GetContacts command.
/// </summary>
public class GetContactsResult : IXmlParseableWithElementName
{
    /// <summary>
    /// The registered domain name.
    /// </summary>
    public string Domain       { get; private set; } = "";
    
    /// <summary>
    /// A unique integer value that represents the domain.
    /// </summary>
    public long    DomainID { get; private set; }
    
    /// <summary>
    /// The real registrant contact.
    /// </summary>
    public Contact  Registrant { get; }              = new();
    
    /// <summary>
    /// The real tech contact.
    /// </summary>
    public Contact  Tech       { get; }              = new();
    
    /// <summary>
    /// The real admin contact.
    /// </summary>
    public Contact  Admin      { get; }              = new();
    
    /// <summary>
    /// The real aux billing contact.
    /// </summary>
    public Contact  AuxBilling { get; }              = new();
    
    /// <summary>
    /// The real billing contact (if set).
    /// </summary>
    public Contact? Billing    { get; private set; } = null;

    /// <summary>
    /// The fake registrant contact (if privacy is on).
    /// </summary>
    public Contact? PrivateRegistrant { get; private set; } = null;
    
    /// <summary>
    /// The fake tech contact (if privacy is on).
    /// </summary>
    public Contact? PrivateTech       { get; private set; } = null;
    
    /// <summary>
    /// The fake admin contact (if privacy is on).
    /// </summary>
    public Contact? PrivateAdmin      { get; private set; } = null;
    
    /// <summary>
    /// The fake aux billing contact (if privacy is on).
    /// </summary>
    public Contact? PrivateAuxBilling { get; private set; } = null;
    
    /// <summary>
    /// The fake billing contact (if privacy is on).
    /// </summary>
    public Contact? PrivateBilling    { get; private set; } = null;

    /// <summary>
    /// The registrant nexus.
    /// </summary>
    public string RegistrantNexus        { get; private set; } = "";
    
    /// <summary>
    /// The registrant nexus country.
    /// </summary>
    public string RegistrantNexusCountry { get; private set; } = "";
    
    /// <summary>
    /// The registrant purpose.
    /// </summary>
    public string RegistrantPurpose      { get; private set; } = "";

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Domain       = element.GetAttribute("Domain");
        DomainID = element.GetAttributeAsInt64("domainnameid");

        ((IXmlParseable)Registrant).LoadFromXmlElement(element.GetChild("Registrant") ?? throw new InvalidOperationException("Missing content."));
        ((IXmlParseable)Tech).LoadFromXmlElement(element.GetChild("Tech") ?? throw new InvalidOperationException("Missing content."));
        ((IXmlParseable)Admin).LoadFromXmlElement(element.GetChild("Admin") ?? throw new InvalidOperationException("Missing content."));
        ((IXmlParseable)AuxBilling).LoadFromXmlElement(element.GetChild("AuxBilling") ?? throw new InvalidOperationException("Missing content."));
        Billing = element.GetChildAs<Contact>("Billing");

        if (element.GetChild("CurrentAttributes") is { } curAttrib)
        {
            RegistrantNexus        = curAttrib.GetChildContent("RegistrantNexus");
            RegistrantNexusCountry = curAttrib.GetChildContent("RegistrantNexusCountry");
            RegistrantPurpose      = curAttrib.GetChildContent("RegistrantPurpose");
        }
        else
        {
            RegistrantNexus        = "";
            RegistrantNexusCountry = "";
            RegistrantPurpose      = "";
        }

        if (element.GetChild("WhoisGuardContact") is { } privateContacts)
        {
            PrivateRegistrant = privateContacts.GetChildAs<Contact>("Registrant");
            PrivateTech       = privateContacts.GetChildAs<Contact>("Tech");
            PrivateAdmin      = privateContacts.GetChildAs<Contact>("Admin");
            PrivateAuxBilling = privateContacts.GetChildAs<Contact>("AuxBilling");
            PrivateBilling    = privateContacts.GetChildAs<Contact>("Billing");
        }
        else
        {
            PrivateRegistrant = null;
            PrivateTech       = null;
            PrivateAdmin      = null;
            PrivateAuxBilling = null;
            PrivateBilling    = null;
        }
    }

    string IXmlParseableWithElementName.ElementName => "DomainContactsResult";
}
