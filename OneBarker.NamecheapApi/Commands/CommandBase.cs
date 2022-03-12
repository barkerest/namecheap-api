namespace OneBarker.NamecheapApi.Commands;

/// <summary>
/// The base command class.
/// </summary>
public abstract class CommandBase : IApiCommand
{
    private readonly string _host;
    private readonly string _apiUri;
    private readonly string _apiUser;
    private readonly string _apiKey;
    private readonly string _userName;
    private readonly string _clientIp;
    private readonly string _command;

    /// <summary>
    /// Initialize the base command configuration.
    /// </summary>
    /// <param name="config"></param>
    /// <param name="command"></param>
    protected CommandBase(IApiConfig config, string command)
    {
        _host     = config.Host;
        _apiUri   = config.ApiUri;
        _apiUser  = config.ApiUser;
        _apiKey   = config.ApiKey;
        _userName = config.UserName;
        _clientIp = config.ClientIp;
        _command  = command;
    }

    /// <inheritdoc />
    string IApiConfig.Host
        => _host;

    /// <inheritdoc />
    string IApiConfig.ApiUri
        => _apiUri;

    /// <inheritdoc />
    string IApiConfig.ApiUser
        => _apiUser;

    /// <inheritdoc />
    string IApiConfig.ApiKey
        => _apiKey;

    /// <inheritdoc />
    string IApiConfig.UserName
        => _userName;

    /// <inheritdoc />
    string IApiConfig.ClientIp
        => _clientIp;

    /// <inheritdoc />
    string IApiCommand.Command
        => _command;

    /// <inheritdoc />
    IEnumerable<KeyValuePair<string, string>> IApiCommand.AdditionalParameters
        => GetAdditionalParameters();

    /// <summary>
    /// Enumerate the additional parameters for this command.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters();
}
