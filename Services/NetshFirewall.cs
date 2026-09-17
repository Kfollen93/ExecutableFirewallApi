using ExecutableFirewallApi.Models;
using ExecutableFirewallApi.Models.Enums;
using ExecutableFirewallApi.Models.Responses;

namespace ExecutableFirewallApi.Services
{
    public class NetshFirewall : IFirewall
    {
        public async Task<FirewallBlockOutcome> BlockAsync(string programPath)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<FirewallRuleInfo>> GetManagedRulesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FirewallStatus> GetStatusAsync(string programPath)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnblockAsync(string programPath)
        {
            throw new NotImplementedException();
        }
    }
}
