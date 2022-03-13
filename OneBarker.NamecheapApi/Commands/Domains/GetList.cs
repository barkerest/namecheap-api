using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Get a list of domains for the configured user.
/// </summary>
public class GetList : CommandBase, IApiCommand<GetListResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public GetList(IApiConfig config)
        : base(config, "namecheap.domains.getList")
    {
    }

    /// <summary>
    /// The page to return.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of records per page (10-100).
    /// </summary>
    [Range(10, 100)]
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// The type of records to return.
    /// </summary>
    public OptionsForListType ListType { get; set; } = OptionsForListType.All;

    /// <summary>
    /// The sort order for the returned records.
    /// </summary>
    public OptionsForSortBy SortBy { get; set; } = OptionsForSortBy.Name;

    /// <summary>
    /// A keyword to look for in the domain list.
    /// </summary>
    [StringLength(70)]
    public string SearchTerm { get; set; } = "";

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("Page", Page.ToString());
        yield return new KeyValuePair<string, string>("PageSize", PageSize.ToString());
        yield return new KeyValuePair<string, string>("ListType", ListType.ToString().ToUpper());
        yield return new KeyValuePair<string, string>("SortBy", SortBy.ToString().ToUpper());
        if (!string.IsNullOrWhiteSpace(SearchTerm)) yield return new KeyValuePair<string, string>("SearchTerm", SearchTerm);
    }
}
