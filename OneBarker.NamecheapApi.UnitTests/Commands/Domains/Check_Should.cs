using System;
using System.Linq;
using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

public class Check_Should : CommonTestBase<Check>
{
    public Check_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override Check CreateValidCommand()
    {
        return new Check(GoodConfig) { DomainList = new []{ "example.com" } };
    }

    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    public void PermitValidNumbersOfDomains(int num)
        => TestValidOption(x => x.DomainList, Enumerable.Range(1, num).Select(x => $"{x}example.com").ToArray());

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void RejectInvalidNumbersOfDomains(int num)
        => TestInvalidOption(x => x.DomainList, num == 0 ? Array.Empty<string>() : Enumerable.Range(1, num).Select(x => $"{x}example.com").ToArray());

    [Theory]
    [InlineData("a")]
    [InlineData("hello.world")]
    [InlineData(Valid70CharString)]
    public void PermitValidDomains(string dom)
        => TestValidOption(x => x.DomainList, new[] { dom });

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(Invalid71CharString)]
    public void RejectInvalidDomains(string dom)
        => TestInvalidOption(x => x.DomainList, new[] { dom });

    [Fact]
    public void RejectRepeatingDomains()
        => TestInvalidOption(x => x.DomainList, new[] { "example.com", "Example.com" });
}
