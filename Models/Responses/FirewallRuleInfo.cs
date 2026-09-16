namespace ExecutableFirewallApi.Models.Responses
{
    public record FirewallRuleInfo(string Id, string RuleName, string ProgramPath, string Direction, bool FileExists);
}
