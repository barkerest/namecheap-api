using Microsoft.Extensions.Logging;
using OneBarker.NamecheapApi.Commands.Domains.Dns;
using OneBarker.NamecheapApi.CommonModels;
using OneBarker.NamecheapApi.Utility;

namespace OneBarker.NamecheapApi.LiveTests.CommandTests.Domains.Dns;

public class GetSetHosts : IApiTester
{
    public string Name => "Domains:Dns:GetSetHosts";

    public void RunTest(ILogger logger)
    {
        var getter = new GetHosts(Program.Config)
        {
            DomainName = Program.TestDomain
        };
        var setter = new SetHosts(Program.Config)
        {
            DomainName = Program.TestDomain
        };

        var current = getter.GetResult();

        logger.LogInformation($"The domain {Program.TestDomain} currently has {current.Count} host records.");

        logger.LogDebug(string.Join("\r\n", current.Select(x => $"{x.RecordType} {x.HostName} {x.Address}")));

        foreach (var entry in current) setter.HostEntries.Add(entry);

        var defEntry = setter.HostEntries.FirstOrDefault(x => x.RecordType == DnsHostEntryRecordType.A && x.HostName == "@");

        if (defEntry is null)
        {
            logger.LogDebug("Adding default @ record.");
            setter.HostEntries.Add(new DnsHostEntry() { HostName = "@", Address = "1.2.3.4", RecordType = DnsHostEntryRecordType.A, TTL = 3600 });
        }
        else if (defEntry.Address == "1.2.3.4")
        {
            logger.LogDebug("Changing default @ record address.");
            setter.HostEntries.Remove(defEntry);
            setter.HostEntries.Add(new DnsHostEntry() { HostName = "@", Address = "4.3.2.1", RecordType = DnsHostEntryRecordType.A, TTL = 3600 });
        }
        else
        {
            logger.LogDebug("Removing default @ record.");
            setter.HostEntries.Remove(defEntry);
        }

        var txtEntry = setter.HostEntries.FirstOrDefault(x => x.RecordType == DnsHostEntryRecordType.TXT && x.HostName == "@" && x.Address.StartsWith("OneB_Test_"));

        if (txtEntry is null)
        {
            logger.LogDebug("Adding OneB_Test_ TXT record.");
            setter.HostEntries.Add(new DnsHostEntry() { RecordType = DnsHostEntryRecordType.TXT, HostName = "@", Address = "OneB_Test_" + Program.RandomNumber(0, int.MaxValue).ToString("X8"), TTL = 3600 });
        }
        else
        {
            logger.LogDebug("Changing OneB_Test_ TXT record.");
            setter.HostEntries.Remove(txtEntry);
            setter.HostEntries.Add(new DnsHostEntry() { RecordType = DnsHostEntryRecordType.TXT, HostName = "@", Address = "OneB_Test_" + Program.RandomNumber(0, int.MaxValue).ToString("X8"), TTL = 3600 });
        }

        var resultOfSet = setter.GetResult();
        if (!resultOfSet.Success)
        {
            throw new ApplicationException("Failed to set host records.");
        }

        current = getter.GetResult();

        var newDefEntry = current.FirstOrDefault(x => x.RecordType == DnsHostEntryRecordType.A && x.HostName == "@");
        var newTxtEntry = current.FirstOrDefault(x => x.RecordType == DnsHostEntryRecordType.TXT && x.HostName == "@" && x.Address.StartsWith("OneB_Test_"));
        if (defEntry is null)
        {
            if (newDefEntry is null) throw new ApplicationException("Failed to set new default entry.");
            if (newDefEntry.Address != "1.2.3.4") throw new ApplicationException("New default host should be at 1.2.3.4.");
        }
        else if (defEntry.Address == "1.2.3.4")
        {
            if (newDefEntry is null) throw new ApplicationException("Failed to change default entry.");
            if (newDefEntry.Address != "4.3.2.1") throw new ApplicationException("New default host should be at 4.3.2.1.");
        }
        else
        {
            if (newDefEntry is not null) throw new ApplicationException("Failed to remove default entry.");
        }

        if (txtEntry is null)
        {
            if (newTxtEntry is null) throw new ApplicationException("Failed to create TXT record.");
        }
        else if (newTxtEntry is null)
        {
            throw new ApplicationException("Changed TXT record is missing.");
        }
        else if (txtEntry.Address == newTxtEntry.Address)
        {
            throw new ApplicationException("Failed to change TXT record.");
        }

        logger.LogInformation("Successfully performed host record maintenance.");
    }
}
