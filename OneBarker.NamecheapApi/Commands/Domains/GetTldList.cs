using OneBarker.NamecheapApi.Results.Domains;

namespace OneBarker.NamecheapApi.Commands.Domains;

public class GetTldList : CommandBase, IApiCommandWithSingleResult<GetTldListResult>
{
    public GetTldList(IApiConfig config)
        : base(config, "namecheap.domains.getTldList")
    {
    }

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield break;
    }
}
