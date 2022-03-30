using System.Collections;
using System.Xml;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains.Dns;

/// <summary>
/// The result from the Domains:DNS:GetHosts command.
/// </summary>
public class GetHostsResult : IXmlParseableWithElementName, IReadOnlyList<DnsHostEntry>
{
    private readonly List<DnsHostEntry> _data = new();
    
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        _data.Clear();

        DomainName  = element.GetAttribute("Domain");
        UsingOurDns = element.GetAttributeAsBoolean("IsUsingOurDNS");

        foreach (var child in element.ChildNodes.OfType<XmlElement>().Where(x => x.Name.ToLower() == "host"))
        {
            _data.Add(child.To<DnsHostEntry>());
        }
    }

    string IXmlParseableWithElementName.ElementName => "DomainDNSGetHostsResult";

    /// <summary>
    /// The domain name for which you are trying to get the host records.
    /// </summary>
    public string DomainName { get; private set; } = "";
    
    /// <summary>
    /// Indicates whether Namecheap's default nameservers are used.
    /// </summary>
    public bool UsingOurDns { get; private set; }

    
    IEnumerator<DnsHostEntry> IEnumerable<DnsHostEntry>.GetEnumerator() => _data.GetEnumerator();
    IEnumerator IEnumerable.                            GetEnumerator() => _data.GetEnumerator();

    
    /// <inheritdoc />
    public int Count => _data.Count;

    /// <inheritdoc />
    public DnsHostEntry this[int index] => _data[index];
}
