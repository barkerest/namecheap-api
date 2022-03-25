namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// An error response from the API.
/// </summary>
public class ApiException : ApplicationException
{
    /// <summary>
    /// One or more errors from the API request.
    /// </summary>
    public IReadOnlyList<ErrorMessage> Errors { get; }
    
    internal ApiException(IEnumerable<ErrorMessage> errors)
    {
        Errors = errors.ToArray();
    }

}
