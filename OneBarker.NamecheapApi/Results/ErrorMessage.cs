using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// An error message from the API.
/// </summary>
public class ErrorMessage : IXmlParseable
{
    /// <summary>
    /// The error number.
    /// </summary>
    public int Number { get; private set; }

    /// <summary>
    /// The error message.
    /// </summary>
    public string Message { get; private set; } = "";


    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Number  = element.GetAttributeAsInt32("Number");
        Message = element.GetContent();
    }
}
