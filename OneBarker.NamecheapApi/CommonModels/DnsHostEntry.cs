using System.ComponentModel.DataAnnotations;
using System.Xml;
using OneBarker.NamecheapApi.Commands.Params;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.CommonModels;

public class DnsHostEntry : ICommandParam, IXmlParseable
{
    [Required]
    public string HostName { get; set; } = "";

    public DnsHostEntryRecordType RecordType { get; set; } = DnsHostEntryRecordType.A;

    [Required]
    public string Address { get; set; } = "";

    public int MxPref { get; set; }
    
    public int TTL { get; set; }

    IEnumerable<KeyValuePair<string, string>> ICommandParam.GenerateParameters(string prefix, string postfix)
    {
        yield return new KeyValuePair<string, string>(prefix + "HostName" + postfix, HostName);
        yield return new KeyValuePair<string, string>(prefix + "RecordType" + postfix, RecordType.ToString().ToUpper());
        yield return new KeyValuePair<string, string>(prefix + "Address" + postfix, Address);
        if (RecordType == DnsHostEntryRecordType.MX)
        {
            yield return new KeyValuePair<string, string>(prefix + "MXPref" + postfix, MxPref.ToString());
        }

        yield return new KeyValuePair<string, string>(prefix + "TTL" + postfix, TTL.ToString());
    }

    void IXmlParseable.LoadFromXmlElement(XmlElement element)
    {
        HostName   = element.GetAttribute("Name");
        RecordType = element.GetAttributeAsEnum<DnsHostEntryRecordType>("Type");
        Address    = element.GetAttribute("Address");
        MxPref     = element.GetAttributeAsInt32("MXPref");
        TTL        = element.GetAttributeAsInt32("TTL");
    }
}
