using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results.Domains;

/// <summary>
/// The result from the Domains:Check command.
/// </summary>
public class CheckResult : IXmlParseable
{
    /// <summary>
    /// The results from the command.
    /// </summary>
    public IReadOnlyList<CheckResultEntry> Results { get; private set; } = Array.Empty<CheckResultEntry>();

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Results = element.ChildNodes
                         .OfType<XmlElement>()
                         .Where(x => x.Name == "DomainCheckResult")
                         .Select(child => child.To<CheckResultEntry>())
                         .ToArray();
    }
}
