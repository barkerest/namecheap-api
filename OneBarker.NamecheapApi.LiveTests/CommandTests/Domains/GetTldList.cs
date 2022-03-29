using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class GetTldList : IApiTester
{
    public string Name => "Domains:GetTldList";
    
    public void   RunTest(ILogger logger)
    {
        var cmd = new Commands.Domains.GetTldList(Program.Config);

        var result = cmd.GetResult();
        logger.LogInformation($"Loaded information on {result.Count} top level domains.");
        logger.LogDebug(string.Join(", ", result.Select(x => x.Name)));
    }
}
