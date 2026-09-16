namespace ExecutableFirewallApi.Models.Responses
{
    public record BlockResult(int Total, int NewlyBlocked, int AlreadyBlocked, int Failed);
}
