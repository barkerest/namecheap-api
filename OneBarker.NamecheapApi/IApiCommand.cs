namespace OneBarker.NamecheapApi;

/// <summary>
/// The configuration for an API command.
/// </summary>
public interface IApiCommand : IApiConfig
{
    /// <summary>
    /// The command to execute.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// The additional parameters for the command.
    /// </summary>
    public IEnumerable<KeyValuePair<string, string>> AdditionalParameters { get; }
}

/// <summary>
/// The configuration for an API command with a strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCommand<TResponse> : IApiCommand where TResponse : class, IXmlParseable, new()
{
    
}

/// <summary>
/// The configuration for an API command with a single strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCommandWithSingleResult<TResponse> : IApiCommand<TResponse> where TResponse : class, IXmlParseableWithElementName, new()
{
    
}

/// <summary>
/// The configuration for an API command with a list of strongly typed responses.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiCommandWithMultipleResults<TResponse> : IApiCommand<TResponse> where TResponse : class, IXmlParseableWithElementName, new()
{
    
}

/// <summary>
/// An API command that requires the GET method.
/// </summary>
public interface IApiGetCommand : IApiCommand
{
    
}

/// <summary>
/// The configuration for an API command with a strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiGetCommand<TResponse> : IApiCommand<TResponse>, IApiGetCommand where TResponse : class, IXmlParseable, new()
{
    
}

/// <summary>
/// The configuration for an API command with a single strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiGetCommandWithSingleResult<TResponse> : IApiCommandWithSingleResult<TResponse>, IApiGetCommand where TResponse : class, IXmlParseableWithElementName, new()
{
    
}

/// <summary>
/// The configuration for an API command with a list of strongly typed responses.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiGetCommandWithMultipleResults<TResponse> : IApiCommandWithMultipleResults<TResponse>, IApiGetCommand where TResponse : class, IXmlParseableWithElementName, new()
{
    
}

/// <summary>
/// An API command that requires the POST method.
/// </summary>
public interface IApiPostCommand : IApiCommand
{
    
}

/// <summary>
/// The configuration for an API command with a strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiPostCommand<TResponse> : IApiCommand<TResponse>, IApiPostCommand where TResponse : class, IXmlParseable, new()
{
    
}

/// <summary>
/// The configuration for an API command with a single strongly typed response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiPostCommandWithSingleResult<TResponse> : IApiCommandWithSingleResult<TResponse>, IApiPostCommand where TResponse : class, IXmlParseableWithElementName, new()
{
    
}

/// <summary>
/// The configuration for an API command with a list of strongly typed responses.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IApiPostCommandWithMultipleResults<TResponse> : IApiCommandWithMultipleResults<TResponse>, IApiPostCommand where TResponse : class, IXmlParseableWithElementName, new()
{
    
}
