using System.Xml;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// The content of this element was not processed.
/// </summary>
public class UnprocessedContent : IXmlParseable
{
    /// <summary>
    /// The raw XML of the content that was not processed.
    /// </summary>
    public string RawXml { get; private set; } = "";

    /// <summary>
    /// Processes this element into the specified type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Process<T>() where T : class, IXmlParseable, new()
    {
        var xml = new XmlDocument();
        xml.LoadXml(RawXml);
        var ret = new T();
        if (xml.DocumentElement is not null)
            ret.LoadFromXmlElement(xml.DocumentElement);
        return ret;
    }

    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        RawXml = element.OuterXml;
    }
}
