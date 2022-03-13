using System;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.Tests.Commands.Domains;

#pragma warning disable xUnit2013

public class GetList_Should
{
    private readonly ITestOutputHelper _output;

    public GetList_Should(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void ExecuteSuccessfully()
    {
        var cmd   = new GetList(Config.ApiConfigWithLogging(_output));
        Assert.True(cmd.IsValid());
        var response = cmd.Execute();
        _output.WriteLine(response.CommandResponse.RawXml);
        Assert.Equal(OptionsForResponseStatus.Ok, response.Status);
    }

    [Fact]
    public void BeValidForTesting()
    {
        var config = new GetList(Config.ApiConfig);
        var valid  = config.IsValid(out var errors);
        if (!valid)
        {
            _output.WriteLine("Errors:\n  " + string.Join("\n  ", errors));
            throw new ApplicationException("Configuration is not valid for testing.");
        }
        
        _output.WriteLine("The configuration is valid.");
    }
    

    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    [InlineData(101)]
    public void RejectInvalidPageSizes(int pageSize)
    {
        var cmd = new GetList(Config.ApiConfig)
        {
            PageSize = pageSize
        };
        var valid = cmd.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.Contains("PageSize"));
        
        var ex = Assert.ThrowsAny<Exception>(() => cmd.Execute());
        Assert.True(ex is ArgumentException or AggregateException { InnerException: ArgumentException });
    }

    [Theory]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890")]
    public void RejectInvalidSearchTerms(string searchTerm)
    {
        var cmd = new GetList(Config.ApiConfig)
        {
            SearchTerm = searchTerm
        };
        var valid = cmd.IsValid(out var errors);
        Assert.False(valid);
        _output.WriteLine(string.Join("\n", errors));
        Assert.Equal(1, errors.Length);
        Assert.Contains(errors, x => x.Contains("SearchTerm"));
        
        var ex = Assert.ThrowsAny<Exception>(() => cmd.Execute());
        Assert.True(ex is ArgumentException or AggregateException { InnerException: ArgumentException });
    }
}
