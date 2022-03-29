using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class Renew : IApiTester
{
    public string Name => "Domains:Renew";
    
    public void   RunTest(ILogger logger)
    {
        var cmd = new Commands.Domains.Renew(Program.Config)
        {
            DomainName = Program.TestDomain,
            Years      = 1
        };

        var result = cmd.GetResult();
        if (result.DomainName != cmd.DomainName) throw new ApplicationException("Result does not match request.");
        logger.LogInformation($"Renewal of {cmd.DomainName} was {result.Success}.");
    }
}
