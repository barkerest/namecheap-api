using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class GetInfo : IApiTester
{
    public string Name => "Domains:GetInfo";
    
    public void   RunTest(ILogger logger)
    {
        var cmd    = new Commands.Domains.GetInfo(Program.Config) { DomainName = Program.TestDomain };
        var result = cmd.GetResult();
        logger.LogInformation($@"Domain {result.DomainName} status is {result.Status}.
Created {result.CreatedDate} and will expire {result.ExpireDate}.
Whoisguard status is {result.WhoisGuardEnabled}.
Lock details:
{result.LockDetails}
Modification rights:
{result.ModificationRights}");

    }
}
