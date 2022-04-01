using System.ComponentModel.DataAnnotations;
using OneBarker.NamecheapApi.Attributes;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Results.Domains.Dns;

namespace OneBarker.NamecheapApi.Commands.Domains.Dns;

/// <summary>
/// Sets DNS host records settings for the requested domain.
/// </summary>
public class SetHosts : CommandBase, IApiCommandWithSingleResult<SetHostsResult>
{
    /// <summary>
    /// Create a new command.
    /// </summary>
    /// <param name="config">The API configuration.</param>
    public SetHosts(IApiConfig config)
        : base(config, "namecheap.domains.dns.setHosts")
    {
    }

    /// <summary>
    /// SLD of the domain to setHosts.
    /// </summary>
    [Required, StringLength(70)]
    public string SLD { get; set; } = "";

    /// <summary>
    /// TLD of the domain to setHosts.
    /// </summary>
    [Required, StringLength(10)]
    public string TLD { get; set; } = "";

    /// <summary>
    /// Domain name of the domain to setHosts (SLD + '.' + TLD).
    /// </summary>
    [Required, StringLength(70)]
    public string DomainName
    {
        get => (string.IsNullOrWhiteSpace(SLD) && string.IsNullOrWhiteSpace(TLD)) ? "" : (SLD + '.' + TLD);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                SLD = TLD = "";
            }
            else
            {
                var chunks = value.Split('.', 2);
                SLD = chunks[0];
                TLD = chunks.Length < 2 ? "" : chunks[1];
            }
        }
    }

    /// <summary>
    /// Change the mail type for this domain.
    /// </summary>
    public OptionsForSetHostsEmailType? EmailType { get; set; }

    /// <summary>
    /// Flag value
    /// </summary>
    /// <remarks>
    /// Is an unsigned integer between 0 and 255. The flag value is an 8-bit number, the most significant bit of which indicates the criticality of understanding of a record by a CA. It's recommended to use '0'
    /// </remarks>
    public byte? Flag { get; set; }

    /// <summary>
    /// Tag value
    /// </summary>
    /// <remarks>
    /// A non-zero sequence of US-ASCII letters and numbers in lower case. The tag value can be one of the following values:
    ///    issue — specifies the certification authority that is authorized to issue a certificate for the domain name or subdomain record used in the title.
    ///    issuewild — specifies the certification authority that is allowed to issue a wildcard certificate for the domain name  or subdomain  record used in the title. The certificate applies to the domain name or subdomain directly and to all its subdomains.
    ///    iodef — specifies the e-mail address or URL (compliant with RFC 5070) a CA should use to notify a client if any issuance policy violation spotted by this CA.
    /// </remarks>
    public OptionsForSetHostsTag? Tag { get; set; }

    /// <summary>
    /// All of the host records for this domain (entries missing will be removed).
    /// </summary>
    [ListSize(1, 150)]
    public IList<DnsHostEntry> HostEntries { get; } = new List<DnsHostEntry>();

    protected override IEnumerable<KeyValuePair<string, string>> GetAdditionalParameters()
    {
        yield return new KeyValuePair<string, string>("SLD", SLD);
        yield return new KeyValuePair<string, string>("TLD", TLD);

        for (var i = 0; i < HostEntries.Count; i++)
        {
            var n = i + 1;
            foreach (var kv in ((ICommandParam)HostEntries[i]).GenerateParameters("", n.ToString()))
            {
                yield return kv;
            }
        }

        if (EmailType.HasValue) yield return new KeyValuePair<string, string>("EmailType", EmailType.GetValueOrDefault().ToString().ToUpper());
        
        // FIXME: Namecheap accepts CAA records like '0 issue "some.address"'
        if (Flag.HasValue) yield return new KeyValuePair<string, string>("Flag", Flag.GetValueOrDefault().ToString());
        if (Tag.HasValue) yield return new KeyValuePair<string, string>("Tag", Tag.GetValueOrDefault().ToString().ToLower());
    }
}
