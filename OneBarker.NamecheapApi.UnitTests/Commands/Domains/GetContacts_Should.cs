using System.Runtime.InteropServices;
using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class GetContacts_Should : CommonTestBase<GetContacts>
{
    public GetContacts_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override GetContacts CreateValidCommand()
    {
        return new GetContacts(GoodConfig) { DomainName = "example.com" };
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
