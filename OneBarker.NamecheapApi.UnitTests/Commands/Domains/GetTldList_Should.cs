using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class GetTldList_Should : CommonTestBase<GetTldList>
{
    public GetTldList_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override GetTldList CreateValidCommand()
    {
        return new GetTldList(GoodConfig);
    }
    
    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());

}
