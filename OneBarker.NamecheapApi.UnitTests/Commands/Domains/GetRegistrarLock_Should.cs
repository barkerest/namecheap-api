using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class GetRegistrarLock_Should : CommonTestBase<GetRegistrarLock>
{
    public GetRegistrarLock_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override GetRegistrarLock CreateValidCommand()
    {
        return new GetRegistrarLock(GoodConfig) { DomainName = "example.com" };
    }
    
    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());

    [Theory]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid70CharString)]
    public void PermitValidDomains(string dom)
        => TestValidOption(x => x.DomainName, dom);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid71CharString)]
    public void RejectInvalidDomains(string dom)
        => TestInvalidOption(x => x.DomainName, dom);
}
