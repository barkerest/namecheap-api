using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Results;
using OneBarker.NamecheapApi.Results.Domains;
using OneBarker.NamecheapApi.Utility;
using BindingFlags = System.Reflection.BindingFlags;

namespace OneBarker.NamecheapApi.LiveTests;

public static class Program
{
    public static RandomNumberGenerator RNG        { get; }
    public static IApiConfig            Config     { get; }
    public static string                TestDomain { get; private set; } = "";

    
    static Program()
    {
        Config = TestConfig.Config.ApiConfigWithLogging(
            builder => builder
                       .AddConsole()
                       .SetMinimumLevel(LogLevel.Debug)
                       .AddFilter(TestConfig.Config.BasicLoggingFilter)
        );
        
        RNG = RandomNumberGenerator.Create();
    }

    /// <summary>
    /// Generate a random number between min and max inclusive of both min and max.
    /// </summary>
    /// <param name="min">The minimum value to return.</param>
    /// <param name="max">The maximum value to return.</param>
    /// <returns></returns>
    public static int RandomNumber(int min, int max)
    {
        if (max <= min) throw new ArgumentException("Max must be greater than min.");
        var diff  = max - min + 1;
        var bytes = new byte[4];
        RNG.GetBytes(bytes, 0, 4);
        return (int)(BitConverter.ToUInt32(bytes, 0) % diff) + min;
    }
    
    /// <summary>
    /// Returns a random entry from a list and removes the entry from the list.
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns a random entry or null if no more entries exist.</returns>
    public static T? PopRandomEntry<T>(this List<T> list) where T : class
    {
        if (list.Count == 0) return null;
        int index;
        
        if (list.Count == 1)
        {
            index = 0;
        }
        else
        {
            var bytes = new byte[4];
            RNG.GetBytes(bytes, 0, 4);
            index = (int)(BitConverter.ToUInt32(bytes, 0) % list.Count);
        }

        var ret = list[index];
        list.RemoveAt(index);
        return ret;
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

            var testDom                                     = TestConfig.Config.Configuration["NamecheapApi:TestDomainName"] ?? "example";
            var testTld                                     = TestConfig.Config.Configuration["NamecheapApi:TestDomainTLD"] ?? "com";
            var explicitTestDomain                          = TestConfig.Config.Configuration["NamecheapApi:ExplicitTestDomain"];

            if (string.IsNullOrWhiteSpace(explicitTestDomain))
            {

                if (string.IsNullOrWhiteSpace(testDom)) testDom = "example";
                if (string.IsNullOrWhiteSpace(testTld)) testTld = "com";

                logger.LogDebug("Selecting a test domain ");
                var n     = RandomNumber(1, 15);
                var check = new Commands.Domains.Check(Config);

                CheckResultEntry? checkResult;

                while (true)
                {
                    check.DomainList = Enumerable
                                       .Range(1, 10)
                                       .Select(
                                           _ =>
                                           {
                                               var ret = $"{testDom}-{n}.{testTld}";
                                               n += RandomNumber(1, 15);
                                               return ret;
                                           }
                                       )
                                       .ToArray();
                    logger.LogDebug("Checking the following domains:\r\n  " + string.Join("\r\n  ", check.DomainList));
                    checkResult = check.GetResult().Results.FirstOrDefault(x => x is { Available: true });
                    if (checkResult is not null) break;
                }

                TestDomain = checkResult.Domain;

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

            }
            else
            {
                TestDomain = explicitTestDomain;
                logger.LogInformation($"User supplied explicit test domain, skipping check/register tests.\r\nTest domain: {TestDomain}");
            }

            var tint    = typeof(IApiTester);
            var testers = typeof(Program)
                          .Assembly
                          .GetTypes()
                          .Where(x => tint.IsAssignableFrom(x))
                          .Select(x => x.GetConstructor(BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes))
                          .Where(x => x is not null)
                          .Select(x => (IApiTester)x.Invoke(null))
                          .OrderBy(x => x.Name)
                          .ToList();

            logger.LogInformation($"Found {testers.Count} testers to run...");

            while (testers.PopRandomEntry() is { } tester)
            {
                logger.LogInformation($"Running {tester.Name}...");
                tester.RunTest(logger);
            }

            logger.LogInformation("All tests complete.");
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
