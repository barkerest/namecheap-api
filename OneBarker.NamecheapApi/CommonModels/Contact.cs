using System.ComponentModel.DataAnnotations;
using System.Xml;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.CommonModels;

/// <summary>
/// Information about a contact.
/// </summary>
public class Contact : ICommandParam, IXmlParseable
{
    /// <summary>
    /// The organization name for this contact.
    /// </summary>
    [StringLength(255)]
    public string OrganizationName { get; set; } = "";

    /// <summary>
    /// The job title for this contact.
    /// </summary>
    [StringLength(255)]
    public string JobTitle { get; set; } = "";

    /// <summary>
    /// The first name for this contact.
    /// </summary>
    [Required, StringLength(255)]
    public string FirstName { get; set; } = "";

    /// <summary>
    /// The last name for this contact.
    /// </summary>
    [Required, StringLength(255)]
    public string LastName { get; set; } = "";

    /// <summary>
    /// First line of the address for this contact.
    /// </summary>
    [Required, StringLength(255)]
    public string Address1 { get; set; } = "";

    /// <summary>
    /// Second line of the address for this contact.
    /// </summary>
    [StringLength(255)]
    public string Address2 { get; set; } = "";

    /// <summary>
    /// City for this contact.
    /// </summary>
    [Required, StringLength(50)]
    public string City { get; set; } = "";

    /// <summary>
    /// The state or province for this contact.
    /// </summary>
    [Required, StringLength(50)]
    public string StateOrProvince { get; set; } = "";

    /// <summary>
    /// The state or province choice for this contact.
    /// </summary>
    [StringLength(50)]
    public string StateOrProvinceChoice { get; set; } = "";

    /// <summary>
    /// The postal code for this contact.
    /// </summary>
    [Required, StringLength(50)]
    public string PostalCode { get; set; } = "";

    /// <summary>
    /// The country for this contact.
    /// </summary>
    [Required, StringLength(50)]
    public string Country { get; set; } = "";

    /// <summary>
    /// The phone number for this contact.
    /// </summary>
    [Required, StringLength(50)]
    public string Phone { get; set; } = "";

    /// <summary>
    /// The phone extension for this contact.
    /// </summary>
    [StringLength(50)]
    public string PhoneExt { get; set; } = "";

    /// <summary>
    /// The fax number for this contact.
    /// </summary>
    [StringLength(50)]
    public string Fax { get; set; } = "";

    /// <summary>
    /// The email address for this contact.
    /// </summary>
    [Required, StringLength(255)]
    public string EmailAddress { get; set; } = "";

    /// <summary>
    /// Indicates if the contact information is read-only.  Set on return from GetContacts.
    /// </summary>
    public bool ReadOnly { get; set; }


    /// <summary>
    /// Copies from the other contact into this contact.
    /// </summary>
    /// <param name="other"></param>
    public void CopyFrom(Contact other)
    {
        OrganizationName      = other.OrganizationName;
        JobTitle              = other.JobTitle;
        FirstName             = other.FirstName;
        LastName              = other.LastName;
        Address1              = other.Address1;
        Address2              = other.Address2;
        City                  = other.City;
        StateOrProvince       = other.StateOrProvince;
        StateOrProvinceChoice = other.StateOrProvinceChoice;
        PostalCode            = other.PostalCode;
        Country               = other.Country;
        Phone                 = other.Phone;
        PhoneExt              = other.PhoneExt;
        Fax                   = other.Fax;
        EmailAddress          = other.EmailAddress;
        ReadOnly              = other.ReadOnly;
    }
    
    /// <inheritdoc />
    IEnumerable<KeyValuePair<string, string>> ICommandParam.GenerateParameters(string prefix)
    {
        if (!string.IsNullOrWhiteSpace(OrganizationName)) yield return new KeyValuePair<string, string>(prefix + "OrganizationName", OrganizationName);
        if (!string.IsNullOrWhiteSpace(JobTitle)) yield return new KeyValuePair<string, string>(prefix + "JobTitle", JobTitle);
        yield return new KeyValuePair<string, string>(prefix + "FirstName", FirstName);
        yield return new KeyValuePair<string, string>(prefix + "LastName", LastName);
        yield return new KeyValuePair<string, string>(prefix + "Address1", Address1);
        if (!string.IsNullOrWhiteSpace(Address2)) yield return new KeyValuePair<string, string>(prefix + "Address2", Address2);
        yield return new KeyValuePair<string, string>(prefix + "City", City);
        yield return new KeyValuePair<string, string>(prefix + "StateProvince", StateOrProvince);
        if (!string.IsNullOrWhiteSpace(StateOrProvinceChoice)) yield return new KeyValuePair<string, string>(prefix + "StateProvinceChoice", StateOrProvinceChoice);
        yield return new KeyValuePair<string, string>(prefix + "PostalCode", PostalCode);
        yield return new KeyValuePair<string, string>(prefix + "Country", Country);
        yield return new KeyValuePair<string, string>(prefix + "Phone", Phone);
        if (!string.IsNullOrWhiteSpace(PhoneExt)) yield return new KeyValuePair<string, string>(prefix + "PhoneExt", PhoneExt);
        if (!string.IsNullOrWhiteSpace(Fax)) yield return new KeyValuePair<string, string>(prefix + "Fax", Fax);
        yield return new KeyValuePair<string, string>(prefix + "EmailAddress", EmailAddress);
    }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        ReadOnly              = element.GetAttributeAsBoolean("ReadOnly");
        OrganizationName      = element.GetChildContent("OrganizationName");
        JobTitle              = element.GetChildContent("JobTitle");
        FirstName             = element.GetChildContent("FirstName");
        LastName              = element.GetChildContent("LastName");
        Address1              = element.GetChildContent("Address1");
        Address2              = element.GetChildContent("Address2");
        City                  = element.GetChildContent("City");
        StateOrProvince       = element.GetChildContent("StateProvince");
        StateOrProvinceChoice = element.GetChildContent("StateProvinceChoice");
        PostalCode            = element.GetChildContent("PostalCode");
        Country               = element.GetChildContent("Country");
        Phone                 = element.GetChildContent("Phone");
        PhoneExt              = element.GetChildContent("PhoneExt");
        Fax                   = element.GetChildContent("Fax");
        EmailAddress          = element.GetChildContent("EmailAddress");
    }
}
