using System.Xml;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.Results;

/// <summary>
/// A response from an API command.
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class ApiCommandResponse<TResult> : IApiResponse<TResult> where TResult : class, IXmlParseable, new()
{
    /// <inheritdoc />
    public OptionsForResponseStatus Status { get; private set; }

    /// <inheritdoc />
    public IReadOnlyList<ErrorMessage> Errors { get; private set; } = Array.Empty<ErrorMessage>();

    /// <inheritdoc />
    public string RequestedCommand { get; private set; } = "";

    /// <inheritdoc />
    public string Server { get; private set; } = "";

    /// <inheritdoc />
    public string GmtTimeDifference { get; private set; } = "";

    /// <inheritdoc />
    public double ExecutionTime { get; private set; }

    /// <inheritdoc />
    public TResult CommandResponse { get; } = new();

    /// <inheritdoc />
    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        Status = element.GetAttributeAsEnum("Status", OptionsForResponseStatus.Unknown);

        foreach (var child in element.ChildNodes.OfType<XmlElement>())
        {
            switch (child.Name)
            {
                case "Errors":
                {
                    var errors = child.ChildNodes.OfType<XmlElement>().ToArray();
                    if (errors.Any())
                    {
                        Errors = errors
                                 .Select(x => x.To<ErrorMessage>())
                                 .ToArray();
                    }
                }
                    break;
                case "Server":
                    Server = child.GetContent();
                    break;
                case "GMTTimeDifference":
                    GmtTimeDifference = child.GetContent();
                    break;
                case "ExecutionTime":
                    ExecutionTime = child.GetContentAsDouble();
                    break;
            }
        }

        if (Status == OptionsForResponseStatus.Ok)
        {
            var responseData = element.GetChild("CommandResponse")
                               ?? throw new InvalidOperationException("The CommandResponse element is missing from the response.");

            CommandResponse.LoadFromXmlElement(responseData);
        }
    }
}
