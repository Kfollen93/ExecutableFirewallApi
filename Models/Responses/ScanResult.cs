namespace ExecutableFirewallApi.Models.Responses
{
    public record ScanResult(string FolderPath, int ExecutableCount, IReadOnlyCollection<ExecutableInfo> Executables);
}
