namespace OneBarker.NamecheapApi.Commands.Params;

/// <summary>
/// A command parameter model.
/// </summary>
public interface ICommandParam
{
    /// <summary>
    /// Enumerates the parameters with the supplied prefix on the key names.
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="postfix"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<string, string>> GenerateParameters(string prefix, string postfix);
}
