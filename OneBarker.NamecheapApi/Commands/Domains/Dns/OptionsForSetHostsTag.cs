namespace OneBarker.NamecheapApi.Commands.Domains.Dns;

public enum OptionsForSetHostsTag
{
    /// <summary>
    ///  specifies the certification authority that is authorized to issue a certificate for the domain name or subdomain record used in the title. 
    /// </summary>
    Issue,
    
    /// <summary>
    /// specifies the certification authority that is allowed to issue a wildcard certificate for the domain name or subdomain record used in the title.
    /// </summary>
    IssueWild,
    
    /// <summary>
    /// specifies the e-mail address or URL (compliant with RFC 5070) a CA should use to notify a client if any issuance policy violation spotted by this CA.
    /// </summary>
    IoDef
    
}
