using System;
using OneBarker.NamecheapApi.Commands.Domains;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Utility;
using Xunit;
using Xunit.Abstractions;

namespace OneBarker.NamecheapApi.Tests.Commands.Domains;

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
        var valid = cmd.IsValid(out var errors);
        if (!valid)
        {
            _output.WriteLine("Bad Config:");
            _output.WriteLine(string.Join("\n", errors));
            throw new ApplicationException("Configuration is not valid for testing.");
        }

        var response = cmd.Execute();
        Assert.Equal(OptionsForResponseStatus.Ok, response.Status);
        
    }
}
