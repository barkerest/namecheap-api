using System.Text;
using System.Web;
using System.Xml;
using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Results;

namespace OneBarker.NamecheapApi.Utility;

/// <summary>
/// Extension methods for API commands.
/// </summary>
public static class ApiCommandExtensions
{
    /// <summary>
    /// The client used to execute commands.
    /// </summary>
    private static readonly HttpClient Client = new();

    
    /// <summary>
    /// Gets the request URI for the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="maskUserAndKey"></param>
    /// <returns></returns>
    public static string ToRequestUri(this IApiCommand command, bool maskUserAndKey = false)
    {
        var safeApiUser  = maskUserAndKey ? "+API-USER+" : HttpUtility.UrlEncode(command.ApiUser);
        var safeApiKey   = maskUserAndKey ? "+API-KEY+" : HttpUtility.UrlEncode(command.ApiKey);
        var safeUserName = maskUserAndKey ? "+USER-NAME+" : HttpUtility.UrlEncode(command.UserName);
        
        var uriBuilder = new StringBuilder();
        uriBuilder.Append(command.ApiUri)
                  .Append("?ApiUser=")
                  .Append(safeApiUser)
                  .Append("&ApiKey=")
                  .Append(safeApiKey)
                  .Append("&Command=")
                  .Append(HttpUtility.UrlEncode(command.Command))
                  .Append("&UserName=")
                  .Append(safeUserName)
                  .Append("&ClientIp=")
                  .Append(HttpUtility.UrlEncode(command.ClientIp));

        foreach (var (key, value) in command.AdditionalParameters)
        {
            uriBuilder.Append('&')
                      .Append(HttpUtility.UrlEncode(key))
                      .Append('=')
                      .Append(HttpUtility.UrlEncode(value));
        }

        return uriBuilder.ToString();
    }
    
    // TODO: Rate limiting?

    private static async Task<XmlElement> ExecuteForXmlAsync(this IApiCommand command, ILogger logger)
    {
        var uri = command.ToRequestUri();
        
        logger.LogDebug($"Sending request\n{command.ToRequestUri(true)}");
        var response    = await Client.GetStringAsync(uri);
        
        logger.LogDebug("Parsing XML response...");
        var xml = new XmlDocument();
        xml.LoadXml(response);

        if (xml.DocumentElement is null)
            throw new XmlException("No XML content generated.");
        
        return xml.DocumentElement;
    }
    
    /// <summary>
    /// Executes the command and returns the response for processing.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static async Task<ApiCommandResponse<UnprocessedContent>> ExecuteAsync(this IApiCommand command)
    {
        var logger  = command.LoggerFactory.CreateLogger(command.Command);
        var element = await command.ExecuteForXmlAsync(logger);
        
        var ret = new ApiCommandResponse<UnprocessedContent>();
        ((IXmlParseable)ret).LoadFromXmlElement(element);
        logger.LogInformation($"Result was {ret.Status}.");
        
        return ret;
    }

    /// <summary>
    /// Executes the command and returns the response for processing.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static ApiCommandResponse<UnprocessedContent> Execute(this IApiCommand command)
    {
        var logger  = command.LoggerFactory.CreateLogger(command.Command);
        var element = command.ExecuteForXmlAsync(logger).Result;

        var ret = new ApiCommandResponse<UnprocessedContent>();
        ((IXmlParseable)ret).LoadFromXmlElement(element);
        logger.LogInformation($"Result was {ret.Status}.");
        
        return ret;
    }

    /// <summary>
    /// Executes the command and returns the result.
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<TResult> GetResultAsync<TResult>(this IApiCommand<TResult> command) where TResult : class, IXmlParseable, new()
    {
        var logger  = command.LoggerFactory.CreateLogger(command.Command);
        var element = await command.ExecuteForXmlAsync(logger);

        var ret = new ApiCommandResponse<TResult>();
        ((IXmlParseable)ret).LoadFromXmlElement(element);
        logger.LogInformation($"Result was {ret.Status}.");

        // TODO: Process errors!
        
        return ret.CommandResponse;
    }
    
    /// <summary>
    /// Executes the command and returns the result.
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult GetResult<TResult>(this IApiCommand<TResult> command) where TResult : class, IXmlParseable, new()
    {
        var logger  = command.LoggerFactory.CreateLogger(command.Command);
        var element = command.ExecuteForXmlAsync(logger).Result;

        var ret = new ApiCommandResponse<TResult>();
        ((IXmlParseable)ret).LoadFromXmlElement(element);
        logger.LogInformation($"Result was {ret.Status}.");

        // TODO: Process errors!
        
        return ret.CommandResponse;
    }

}
