using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// A response containing a single result.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleResult<T> : IXmlParseable where T : class, IXmlParseableWithElementName, new()
{
    /// <summary>
    /// The result from the command.
    /// </summary>
    public T Result { get; private set; } = new();

    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Result = element.GetChildAs<T>(Result.ElementName) ?? throw new InvalidOperationException("The result is missing from the response.");
    }
}
