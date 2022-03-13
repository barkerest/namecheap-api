using System.Xml;

namespace OneBarker.NamecheapApi;

/// <summary>
/// Interface for parseable return values.
/// </summary>
public interface IXmlParseable
{
    /// <summary>
    /// Fill the properties of this item from the current XML element.
    /// </summary>
    /// <param name="element"></param>
    public void LoadFromXmlElement(XmlElement element);
}

/// <summary>
/// Interface for parseable single values.
/// </summary>
public interface IXmlParseableWithElementName : IXmlParseable
{
    /// <summary>
    /// The name of the element that contains this item.
    /// </summary>
    public string ElementName { get; }
}
