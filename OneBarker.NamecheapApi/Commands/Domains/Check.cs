using OneBarker.NamecheapApi.Attributes;
using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

/// <summary>
/// Checks the availability of domains
/// </summary>
public class Check : CommandBase, IApiCommandWithMultipleResults<CheckResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public Check(IApiConfig config)
        : base(config, "namecheap.domains.check")
    {
    }

    /// <summary>
    /// The domains to check for.
    /// </summary>
    [ListSize(1, 50), ListEntryLength(1, 70), ListUnique(CaseInsensitive = true)]
    public IReadOnlyList<string> DomainList { get; set; } = Array.Empty<string>();

    
    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("DomainList", string.Join(",", DomainList));
    }
}
