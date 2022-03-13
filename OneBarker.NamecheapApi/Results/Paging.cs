using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// Paging information for the returned list.
/// </summary>
public class Paging : IXmlParseable
{
    /// <summary>
    /// The total number of items available.
    /// </summary>
    public int TotalItems  { get; private set; }
    
    /// <summary>
    /// The current page returned.
    /// </summary>
    public int CurrentPage { get; private set; }
    
    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize    { get; private set; }

    /// <summary>
    /// The total number of pages available.
    /// </summary>
    public int TotalPages => PageSize > 0 
                                 ? (TotalItems + PageSize - 1) / PageSize 
                                 : 0;

    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        TotalItems  = element.GetChildContentAsInt32("TotalItems");
        CurrentPage = element.GetChildContentAsInt32("CurrentPage");
        PageSize    = element.GetChildContentAsInt32("PageSize");
    }
    
}
