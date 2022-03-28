using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains;

public class GetSetRegistrarLock : IApiTester
{
    public string Name => "Domains:GetSetRegistrarLock";
    
    public void   RunTest(ILogger logger)
    {
        var getter = new GetRegistrarLock(Program.Config)
        {
            DomainName = Program.TestDomain
        };
        var setter = new SetRegistrarLock(Program.Config)
        {
            DomainName = Program.TestDomain
        };

        var curSetting = getter.GetResult();
        logger.LogInformation($"Current lock status: {curSetting.RegistrarLockStatus}");

        setter.LockAction = curSetting.RegistrarLockStatus ? OptionsForLockAction.Unlock : OptionsForLockAction.Lock;
        var setResult = setter.GetResult();
        if (!setResult.Success) throw new ApplicationException("Failed to set registrar lock status.");
        var expected = !curSetting.RegistrarLockStatus;
        curSetting = getter.GetResult();
        if (curSetting.RegistrarLockStatus != expected)
            throw new ApplicationException("Lock status has not been changed.");
        
        logger.LogInformation("Lock status has been successfully changed.");
    }
}
