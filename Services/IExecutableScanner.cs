using ExecutableFirewallApi.Models;

namespace ExecutableFirewallApi.Services
{
    public interface IExecutableScanner
    {
        IReadOnlyCollection<ExecutableInfo> Scan(string folderPath);
    }
}
