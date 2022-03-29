using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class Reactivate : IApiTester
{
    public string Name => "Domains:Reactivate";
    
    public void   RunTest(ILogger logger)
    {
        // test 1 - should fail.
        try
        {
            var cmd = new Commands.Domains.Reactivate(Program.Config) { DomainName = Program.TestDomain, Years = 1 };

            cmd.GetResult();
            
            throw new ApplicationException($"Reactivation of {Program.TestDomain} should have failed.");
        }
        catch (ApiException e)
        {
            var expected = e.Errors.FirstOrDefault(x => x.Number == 2020166);
            if (expected is null) throw;
            logger.LogInformation($"As expected, reactivation of {Program.TestDomain} failed.\r\n{expected}");
        }
        
        // test 2 - do we have any volunteers.
        var getList    = new Commands.Domains.GetList(Program.Config) { ListType = OptionsForListType.Expired };
        var volunteer = getList.GetResult().Entries.FirstOrDefault();

        if (volunteer is not null)
        {
            logger.LogDebug($"Found an expired domain {volunteer.Name} to reactivate.");
            var reactivate = new Commands.Domains.Reactivate(Program.Config) { DomainName = volunteer.Name, Years = 1 };
            var result     = reactivate.GetResult();
            if (result.Domain != reactivate.DomainName) throw new ApplicationException("Result does not match request.");
            logger.LogInformation($"Renewal of {reactivate.DomainName} was {result.Success}.");
        }
        else
        {
            logger.LogInformation("We have no available expired domains to attempt reactivation with.");
        }
        



    }
}
