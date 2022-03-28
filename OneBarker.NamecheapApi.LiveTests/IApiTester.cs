using Microsoft.Extensions.Logging;

namespace OneBarker.NamecheapApi.LiveTests;

public interface IApiTester
{
    public string Name { get; }
    
    public void RunTest(ILogger logger);
}
