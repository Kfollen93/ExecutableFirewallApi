namespace ExecutableFirewallApi.Models.Responses
{
    public record ExecutableFirewallStatus(string Id, string FileName, string FullPath, bool InboundBlocked, bool OutboundBlocked);
}
