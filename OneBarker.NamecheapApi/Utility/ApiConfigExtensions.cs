using System.Net;
using System.Net.Sockets;

namespace OneBarker.NamecheapApi.Utility;

/// <summary>
/// Extension methods for API configuration.
/// </summary>
public static class ApiConfigExtensions
{
    /// <summary>
    /// Tests the configuration for validity.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static bool IsValid(this IApiConfig config, out string[] errors)
    {
        var err = new List<string>();

        if (string.IsNullOrWhiteSpace(config.Host))
        {
            err.Add("Host is not set.");
        }
        else if (config.Host.Length > 255)
        {
            err.Add("Host is limited to 255 characters.");
        }
        else
        {
            try
            {
                var addresses = Dns.GetHostAddresses(config.Host);
                if (addresses.Length < 1)
                {
                    err.Add("Host does not resolve.");
                }
            }
            catch (Exception e) when (e is ArgumentException or SocketException)
            {
                err.Add("Host is not a valid host name.");
            }
        }

        if (string.IsNullOrWhiteSpace(config.ApiUser))
        {
            err.Add("ApiUser is not set.");
        }
        else if (config.ApiUser.Length > 20)
        {
            err.Add("ApiUser is limited to 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(config.ApiKey))
        {
            err.Add("ApiKey is not set.");
        }
        else if (config.ApiKey.Length > 50)
        {
            err.Add("ApiKey is limited to 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(config.UserName))
        {
            err.Add("UserName is not set.");
        }
        else if (config.UserName.Length > 20)
        {
            err.Add("UserName is limited to 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(config.ClientIp))
        {
            err.Add("ClientIP is not set.");
        }
        else if (IPAddress.TryParse(config.ClientIp, out var ip))
        {
            if (ip.AddressFamily != AddressFamily.InterNetwork)
            {
                err.Add("ClientIP is not an IPv4 address.");
            }
        }
        else
        {
            err.Add("ClientIP is not a valid IP address.");
        }

        if (string.IsNullOrWhiteSpace(config.ApiUri))
        {
            err.Add("ApiUri is not set.");
        }
        else if (!Uri.TryCreate(config.ApiUri, UriKind.Absolute, out _))
        {
            err.Add("ApiUri is not a valid URI.");
        }

        if (config is IApiRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Command))
            {
                err.Add("Command is not set.");
            }
            else if (request.Command.Length > 80)
            {
                err.Add("Command is limited to 80 characters.");
            }
        }
        
        errors = err.ToArray();

        return err.Count == 0;
    }

    /// <summary>
    /// Tests the configuration for validity.
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static bool IsValid(this IApiConfig config) 
        => IsValid(config, out _);

    
    
    
}
