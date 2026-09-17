using ExecutableFirewallApi.Models;
using ExecutableFirewallApi.Models.Enums;
using ExecutableFirewallApi.Models.Responses;

namespace ExecutableFirewallApi.Services
{
    public interface IFirewall
    {
        Task<FirewallStatus> GetStatusAsync(string programPath);

        Task<FirewallBlockOutcome> BlockAsync(string programPath);

        Task<bool> UnblockAsync(string programPath);

        Task<IReadOnlyCollection<FirewallRuleInfo>> GetManagedRulesAsync();
    }
}
