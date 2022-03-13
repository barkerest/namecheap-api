using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:GetList command.
/// </summary>
public class GetListResult : IXmlParseable
{
    /// <summary>
    /// The entries returned.
    /// </summary>
    public IReadOnlyList<GetListResultEntry> Entries { get; private set; } = Array.Empty<GetListResultEntry>();

    /// <summary>
    /// The paging status.
    /// </summary>
    public Paging Paging { get; } = new();
    
    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        foreach (var child in element.ChildNodes.OfType<XmlElement>())
        {
            switch (child.Name)
            {
                case "DomainGetListResult":
                {
                    var entries = child.ChildNodes.OfType<XmlElement>().ToArray();
                    if (entries.Any())
                    {
                        Entries = entries
                                  .Select(x => x.To<GetListResultEntry>())
                                  .ToArray();
                    }
                }
                    break;
                
                case "Paging":
                    ((IXmlParseable)Paging).LoadFromXmlElement(child);
                    break;
            }
        }
    }
}
