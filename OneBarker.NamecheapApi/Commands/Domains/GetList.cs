namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Get a list of domains for the configured user.
/// </summary>
public class GetList : CommandBase
{
    private int     _pageSize = 20;
    private string? _searchTerm;

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
    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value is < 10 or > 100) throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be between 10 and 100.");
            _pageSize = value;
        }
        
    }

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
    public string? SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (value is not null &&
                value.Length > 70)
                throw new ArgumentException("SearchTerm is limited to 70 characters.", nameof(value));
            
            _searchTerm = value;
        }
    }

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("Page", Page.ToString());
        yield return new KeyValuePair<string, string>("PageSize", PageSize.ToString());
        yield return new KeyValuePair<string, string>("ListType", ListType.ToString().ToUpper());
        yield return new KeyValuePair<string, string>("SortBy", SortBy.ToString().ToUpper());
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            yield return new KeyValuePair<string, string>("SearchTerm", SearchTerm);
        }
    }
}
