using System.Linq;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Commands.Domains.Dns;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains.Dns;

public class SetHosts_Should : CommonTestBase<SetHosts>
{
    public SetHosts_Should(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    protected override SetHosts CreateValidCommand()
    {
        return new SetHosts(GoodConfig)
        {
            SLD         = "example",
            TLD         = "com",
            HostEntries = { new DnsHostEntry() { HostName = "@", Address = "1.2.3.4", RecordType = DnsHostEntryRecordType.A, TTL = 3600 } }
        };
    }
    
    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());

    [Theory]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData(Valid64CharString)]
    public void PermitValidSLDValues(string val)
        => TestValidOption(
            x => x.SLD,
            val,
            x => x.TLD = "12345" // five char TLD with dot is 6-chars, making max SLD len 64 chars in this case.
        );

    [Theory]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("Good10Char")]
    public void PermitValidTLDValues(string val)
        => TestValidOption(
            x => x.TLD,
            val,
            x => x.SLD = new string('a', 59) // same concept as above.
        );

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(Invalid71CharString, true)]
    public void RejectInvalidSLDValues(string val, bool affectsDomainNameAsWell)
        => TestInvalidOption(
            val,
            (_, v) =>
            {
                var ret = CreateValidCommand();
                ret.TLD = "12345"; // 5 chars plus dot to make a 65 char string invalid in the other field.
                ret.SLD = v;
                return ret;
            },
            null,
            true,
            affectsDomainNameAsWell ? new[] { "DomainName", "SLD" } : new[] { "SLD" }
        );

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Bad-11-Char", true)]
    public void RejectInvalidTLDValues(string val, bool affectsDomainNameAsWell)
        => TestInvalidOption(
            val,
            (_, v) =>
            {
                var ret = CreateValidCommand();
                ret.SLD = Valid64CharString;
                ret.TLD = v;
                return ret;
            },
            null,
            true, 
            affectsDomainNameAsWell ? new[]{ "DomainName" , "TLD" } : new[]{"TLD"}
        );

    [Theory]
    [InlineData("1.a")]
    [InlineData("super-domain.x.y.z")]
    [InlineData(Valid64CharString + ".12345")]
    public void PermitValidDomains(string dom)
        => TestValidOption(x => x.DomainName, dom);

    [Theory]
    [InlineData("", new[] { "DomainName", "SLD", "TLD" })]
    [InlineData("  ", new[] { "DomainName", "SLD", "TLD" })]
    [InlineData(".abc", new[] { "SLD" })]
    [InlineData("abc.", new[] { "TLD" })]
    [InlineData(Invalid65CharString + ".12345", new []{"DomainName"})]
    public void RejectInvalidDomains(string dom, string[] props)
        => TestInvalidOption(
            dom,
            (c, v) =>
            {
                var ret = CreateValidCommand();
                ret.DomainName = v;
                return ret;
            },
            null,
            true,
            props
        );

    [Fact]
    public void RejectEmptyHosts()
    {
        var ret = CreateValidCommand();
        ret.HostEntries.Clear();
        var valid = ret.IsValid(out var errors);
        Assert.False(valid);
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.Contains("HostEntries"));
    }
    
    [Fact]
    public void RejectTooManyHosts()
    {
        var ret = CreateValidCommand();
        while (ret.HostEntries.Count <= 150) ret.HostEntries.Add(new DnsHostEntry() { HostName = "www-" + ret.HostEntries.Count, Address = "1.2.3.4", TTL = 3600, RecordType = DnsHostEntryRecordType.A });
        Assert.True(ret.HostEntries.Count > 150);
        var valid = ret.IsValid(out var errors);
        Assert.False(valid);
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.Contains("HostEntries"));
    }

    [Fact]
    public void PermitMaxHosts()
    {
        var ret = CreateValidCommand();
        while (ret.HostEntries.Count < 150) ret.HostEntries.Add(new DnsHostEntry() { HostName = "www-" + ret.HostEntries.Count, Address = "1.2.3.4", TTL = 3600, RecordType = DnsHostEntryRecordType.A });
        Assert.Equal(150, ret.HostEntries.Count);
        var valid = ret.IsValid(out var errors);
        if (!valid)
        {
            OutputHelper.WriteLine("Errors:");
            OutputHelper.WriteLine(string.Join("\r\n", errors));
            throw new CustomAssertException("HostEntries is not valid with 150 entries.");
        }

        OutputHelper.WriteLine("HostEntries is valid with 150 entries.");
    }
}
