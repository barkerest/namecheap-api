using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Results.Domains;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class TestDomainsGetList : IApiTester
{
    public string Name => "Domains:GetList";
    
    public void   RunTest(ILogger logger)
    {
        var cmd = new GetList(Program.Config)
        {
            Page     = 1,
            PageSize = 10,
            ListType = OptionsForListType.All,
            SortBy   = OptionsForSortBy.Name
        };

        var result = cmd.GetResult();
        logger.LogInformation($@"Received {result.Entries.Count} entries from API.
API states that {result.Paging.TotalItems} are available.
At {result.Paging.PageSize} per page, that is {result.Paging.TotalPages} pages.");

        var allDomains = new List<GetListResultEntry>();
        allDomains.AddRange(result.Entries);
        
        while (result.Paging.TotalPages > result.Paging.CurrentPage)
        {
            cmd.Page = result.Paging.CurrentPage + 1;
            result   = cmd.GetResult();
            logger.LogInformation($"Loaded {result.Entries.Count} more on page {result.Paging.CurrentPage}.");
            allDomains.AddRange(result.Entries);
        }

        if (allDomains.All(x => !x.Name.Equals(Program.TestDomain, StringComparison.OrdinalIgnoreCase)))
            throw new ApplicationException($"The test domain ({Program.TestDomain}) is missing from the domains returned by the API.");

        logger.LogInformation($"Successfully loaded {allDomains.Count} domains from API.");
        logger.LogDebug(string.Join("\r\n", allDomains.Select(x => x.Name)));
    }
}
