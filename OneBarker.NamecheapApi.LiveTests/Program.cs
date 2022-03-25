using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests;

public static class Program
{
    public static IApiConfig Config     { get; }
    public static string     TestDomain { get; private set; } = "";

    static Program()
    {
        Config = TestConfig.Config.ApiConfigWithLogging(
            builder => builder
                       .AddConsole()
                       .SetMinimumLevel(LogLevel.Debug)
                       .AddFilter(TestConfig.Config.BasicLoggingFilter)
        );
    }

    public static void Main(string[] args)
    {
        var logger = Config.LoggerFactory.CreateLogger("Program");

        try
        {
            logger.LogInformation("Live tests for OneBarker.NamecheapApi");

            if (!Config.IsValid(out var errors))
            {
                if (string.IsNullOrWhiteSpace(Config.ApiUser) ||
                    string.IsNullOrWhiteSpace(Config.ApiKey))
                {
                    logger.LogError(
                        @"The test configuration is not set.
You need to provide the API user and API key for testing.

There are three ways to provide these values.
1) Via the command line using the --api-user and --api-key switches.
2) Via environment variables ONEB_NAMECHEAPAPI__APIUSER and 
   ONEB_NAMECHEAPAPI__APIKEY.
3) Via [LocalAppData]\OneBarker\test-config.json file.
   eg - {""NamecheapApi"":{""ApiUser"":""somebody"",""ApiKey"":""super-secret""}}
"
                    );
                }
                else
                {
                    logger.LogError("The test configuration is not valid:\n  " + string.Join("\n  ", errors));
                }

                return;
            }

            logger.LogInformation(
                $@"Host:      {Config.Host}
Client IP: {Config.ClientIp}
User:      {Config.ApiUser}
Key:       {Config.ApiKey[..5]}..{Config.ApiKey[^5..]}
"
            );


            logger.LogDebug("Selecting a test domain ");
            var n      = 1;
            var check  = new Commands.Domains.Check(Config);
            var domain = $"example-{n}.com";
            while (true)
            {
                check.DomainList = new[] { domain };
                var checkResult = check.GetResult().Results.First(x => x.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
                if (checkResult is { Available: true }) break;

                n++;
                if (n % 5 == 0) n += 250;
                domain = $"example-{n}.com";
            }

            TestDomain = domain;
            logger.LogInformation($"Registering: {TestDomain}");

            var create = new Commands.Domains.Create(Config)
            {
                DomainName      = TestDomain,
                YearsToRegister = 1,
                Registrant =
                {
                    FirstName       = "John",
                    LastName        = "Doe",
                    Address1        = "1 Center Court",
                    City            = "Cleveland",
                    StateOrProvince = "OH",
                    PostalCode      = "44115",
                    Country         = "US",
                    Phone           = "+1.8005554321",
                    EmailAddress    = "j.doe@example.com",
                },
                Admin =
                {
                    FirstName       = "John",
                    LastName        = "Doe",
                    Address1        = "1 Center Court",
                    City            = "Cleveland",
                    StateOrProvince = "OH",
                    PostalCode      = "44115",
                    Country         = "US",
                    Phone           = "+1.8005554321",
                    EmailAddress    = "j.doe@example.com",
                },
                Tech =
                {
                    FirstName       = "John",
                    LastName        = "Doe",
                    Address1        = "1 Center Court",
                    City            = "Cleveland",
                    StateOrProvince = "OH",
                    PostalCode      = "44115",
                    Country         = "US",
                    Phone           = "+1.8005554321",
                    EmailAddress    = "j.doe@example.com",
                },
                AuxBilling =
                {
                    FirstName       = "John",
                    LastName        = "Doe",
                    Address1        = "1 Center Court",
                    City            = "Cleveland",
                    StateOrProvince = "OH",
                    PostalCode      = "44115",
                    Country         = "US",
                    Phone           = "+1.8005554321",
                    EmailAddress    = "j.doe@example.com",
                },
            };
            var createResult = create.GetResult();
            if (createResult.Result.Registered)
            {
                logger.LogInformation($"Successfully registered test domain: {TestDomain}");
            }
            
            // TODO: Run other tests now.
            
        }
        catch (ApiException ex)
        {
            logger.LogError("API Command Failed\r\n" + string.Join("\r\n", ex.Errors));
        }
        finally
        {
            Config.LoggerFactory.Dispose();
        }
    }
}
