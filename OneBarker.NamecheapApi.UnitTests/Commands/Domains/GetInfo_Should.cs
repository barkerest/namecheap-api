using System.Runtime.InteropServices;
using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class GetInfo_Should : CommonTestBase<GetInfo>
{
    public GetInfo_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override GetInfo CreateValidCommand()
    {
        return new GetInfo(GoodConfig) { DomainName = "example.com" };
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
    [InlineData(Valid255CharString)]
    public void PermitValidHosts(string host)
        => TestValidOption(x => x.HostName, host);

    [Theory]
    [InlineData(Invalid256CharString)]
    public void RejectInvalidHosts(string host)
        => TestInvalidOption(x => x.HostName, host);

}
