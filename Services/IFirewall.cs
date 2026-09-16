using ExecutableFirewallApi.Models;
using ExecutableFirewallApi.Models.Responses;

namespace ExecutableFirewallApi.Services
{
    public interface IFirewall
    {
        Task<FirewallStatus> GetStatusAsync(string programPath);

        Task<bool> BlockAsync(string programPath);

        Task<bool> UnblockAsync(string programPath);

        Task<IReadOnlyCollection<FirewallRuleInfo>> GetManagedRulesAsync();
    }
}
