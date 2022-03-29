using System.Runtime.InteropServices;
using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class Renew_Should : CommonTestBase<Renew>
{
    public Renew_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override Renew CreateValidCommand()
    {
        return new Renew(GoodConfig) { DomainName = "example.com", Years = 1 };
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

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1")]
    [InlineData("a")]
    [InlineData(Valid20CharString)]
    public void PermitValidPromotionCodes(string code)
        => TestValidOption(x => x.PromotionCode, code);

    [Theory]
    [InlineData(Invalid21CharString)]
    public void RejectInvalidPromotionCodes(string code)
        => TestInvalidOption(x => x.PromotionCode, code);
    
    

}
