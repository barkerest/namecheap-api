using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// A response containing multiple results.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MultipleResults<T> : IXmlParseable where T : class, IXmlParseableWithElementName, new()
{
    /// <summary>
    /// The results from the command.
    /// </summary>
    public T[] Results { get; private set; } = Array.Empty<T>();

    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        var elemName = new T().ElementName;
        
        var ret      = new List<T>();
        foreach (var child in element.ChildNodes.OfType<XmlElement>().Where(x => x.Name == elemName))
        {
            var item = new T();
            item.LoadFromXmlElement(child);
            ret.Add(item);
        }

        Results = ret.ToArray();
    }
}
