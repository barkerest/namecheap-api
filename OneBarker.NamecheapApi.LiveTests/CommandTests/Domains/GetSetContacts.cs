using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class GetContacts : IApiTester
{
    public string Name => "Domains:GetSetContacts";
    
    public void   RunTest(ILogger logger)
    {
        var cmdGet = new Commands.Domains.GetContacts(Program.Config)
        {
            DomainName = Program.TestDomain
        };
        var cmdSet = new Commands.Domains.SetContacts(Program.Config)
        {
            DomainName = Program.TestDomain,
        };
        
        var result = cmdGet.GetResult();
        logger.LogInformation(
            $@"Current result for {result.Domain}.
Registrant: {result.Registrant.FirstName} {result.Registrant.LastName} ({result.Registrant.EmailAddress})
" +
            ((result.PrivateRegistrant is not null)
                 ? $"Private: {result.PrivateRegistrant.FirstName} {result.PrivateRegistrant.LastName} ({result.PrivateRegistrant.EmailAddress})"
                 : "No private registrant.")
        );
        

        cmdSet.Registrant.CopyFrom(result.Registrant);
        cmdSet.Admin.CopyFrom(result.Admin);
        cmdSet.Tech.CopyFrom(result.Tech);
        cmdSet.AuxBilling.CopyFrom(result.AuxBilling);
        
        if (result.Registrant.FirstName == "John")
        {
            cmdSet.Registrant.FirstName = "Jane";
            cmdSet.Registrant.LastName  = "Smith";
        }
        else
        {
            cmdSet.Registrant.FirstName = "John";
            cmdSet.Registrant.LastName  = "Doe";
        }

        var setResult = cmdSet.GetResult();
        if (!setResult.Success) throw new ApplicationException("Failed to set contact information.");
        result = cmdGet.GetResult();
        
        logger.LogInformation(
            $@"New result for {result.Domain}.
Registrant: {result.Registrant.FirstName} {result.Registrant.LastName} ({result.Registrant.EmailAddress})
" +
            ((result.PrivateRegistrant is not null)
                 ? $"Private: {result.PrivateRegistrant.FirstName} {result.PrivateRegistrant.LastName} ({result.PrivateRegistrant.EmailAddress})"
                 : "No private registrant.")
        );

        if (result.Registrant.FirstName != cmdSet.Registrant.FirstName ||
            result.Registrant.LastName != cmdSet.Registrant.LastName)
        {
            throw new ApplicationException("Registrant name was not changed.");
        }
    }
}
