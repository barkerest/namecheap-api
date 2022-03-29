using OneBarker.NamecheapApi.Commands.Domains;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.UnitTests.Commands.Domains;

#pragma warning disable xUnit2013

public class GetList_Should : CommonTestBase<GetList>
{
    public GetList_Should(ITestOutputHelper output)
        : base(output)
    {
        
    }
    
    [Fact]
    public void BeValidForTesting()
        => TestValidConfig(CreateValidCommand());

    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    [InlineData(101)]
    public void RejectInvalidPageSizes(int pageSize)
        => TestInvalidOption(x => x.PageSize, pageSize);

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public void PermitValidPageSizes(int pageSize)
        => TestValidOption(x => x.PageSize, pageSize);
    
    [Theory]
    [InlineData(Invalid71CharString)]
    public void RejectInvalidSearchTerms(string searchTerm)
        => TestInvalidOption(x => x.SearchTerm, searchTerm);

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("Hello")]
    [InlineData(Valid70CharString)]
    public void PermitValidSearchTerms(string searchTerm)
        => TestValidOption(x => x.SearchTerm, searchTerm);
    
    protected override GetList CreateValidCommand() => new(GoodConfig);
}

