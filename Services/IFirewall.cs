using ExecutableFirewallApi.Models.Enums;

namespace ExecutableFirewallApi.Services
{
    public interface IFirewall
    {
        Task<FirewallBlockOutcome> BlockAsync(string programPath);
        Task<FirewallUnblockOutcome> UnblockAsync(string programPath);
    }
}
