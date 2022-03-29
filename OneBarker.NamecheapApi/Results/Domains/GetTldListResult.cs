using System.Collections;
using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:GetTldList command.
/// </summary>
public class GetTldListResult : IXmlParseableWithElementName, IReadOnlyList<GetTldListEntry>
{
    private readonly List<GetTldListEntry> _data = new();

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        _data.Clear();
        foreach (var child in element.ChildNodes.OfType<XmlElement>().Where(x => x.Name == "Tld"))
        {
            _data.Add(child.To<GetTldListEntry>());
        }
    }

    string IXmlParseableWithElementName.ElementName => "Tlds";

    IEnumerator<GetTldListEntry> IEnumerable<GetTldListEntry>.GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.                                  GetEnumerator() => _data.GetEnumerator();

    /// <inheritdoc />
    public int Count => _data.Count;

    /// <inheritdoc />
    public GetTldListEntry this[int index] => _data[index];
}
