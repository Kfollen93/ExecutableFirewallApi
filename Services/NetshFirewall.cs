using ExecutableFirewallApi.Models;
using ExecutableFirewallApi.Models.Enums;
using ExecutableFirewallApi.Models.Responses;
using ExecutableFirewallApi.Utilities;
using System.Diagnostics;

namespace ExecutableFirewallApi.Services
{
    // Microsoft Learn: netsh advfirewall
    // https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/netsh-advfirewall?tabs=consecadd%2Cfirewalladd%2Cmainmodeadd%2Cmonitordelete
    public class NetshFirewall : IFirewall
    {
        public async Task<FirewallBlockOutcome> BlockAsync(string programPath)
        {
            string shortHash = PathHash.GetShortPathHash(programPath);
            string fileName = Path.GetFileName(programPath);
            string inboundRuleName = $"ExecutableBlock IN - {fileName} - {shortHash}";
            string outboundRuleName = $"ExecutableBlock OUT - {fileName} - {shortHash}";

            bool inboundExists = await FirewallRuleExistsAsync(inboundRuleName);
            bool outboundExists = await FirewallRuleExistsAsync(outboundRuleName);

            if (inboundExists && outboundExists)
            {
                return FirewallBlockOutcome.AlreadyBlocked;
            }

            bool inboundSuccess = inboundExists || await AddFirewallRuleAsync(inboundRuleName, programPath, "in");
            bool outboundSuccess = outboundExists || await AddFirewallRuleAsync(outboundRuleName, programPath, "out");

            if (inboundSuccess && outboundSuccess)
            {
                return FirewallBlockOutcome.Blocked;
            }

            return FirewallBlockOutcome.Failed;
        }

        public Task<IReadOnlyCollection<FirewallRuleInfo>> GetManagedRulesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FirewallStatus> GetStatusAsync(string programPath)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UnblockAsync(string programPath)
        {
            string shortHash = PathHash.GetShortPathHash(programPath);
            string fileName = Path.GetFileName(programPath);
            string inboundRuleName = $"ExecutableBlock IN - {fileName} - {shortHash}";
            string outboundRuleName = $"ExecutableBlock OUT - {fileName} - {shortHash}";

            bool inboundExists = await FirewallRuleExistsAsync(inboundRuleName);
            bool outboundExists = await FirewallRuleExistsAsync(outboundRuleName);
            bool inBoundUnblocked = true;
            bool outBoundUnblocked = true;

            if (inboundExists)
            {
                inBoundUnblocked = await DeleteFirewallRuleAsync(inboundRuleName);
            }

            if (outboundExists)
            {
                outBoundUnblocked = await DeleteFirewallRuleAsync(outboundRuleName);
            }

            return inBoundUnblocked && outBoundUnblocked;
        }

        private async Task<bool> FirewallRuleExistsAsync(string ruleName)
        {
            try
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo?view=net-10.0
                var startInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.argumentlist?view=net-10.0
                // ArgumentList handles escaping individual command-line arguments.
                startInfo.ArgumentList.Add("advfirewall");
                startInfo.ArgumentList.Add("firewall");
                startInfo.ArgumentList.Add("show");
                startInfo.ArgumentList.Add("rule");
                startInfo.ArgumentList.Add($"name={ruleName}");

                using Process? process = Process.Start(startInfo);

                if (process == null)
                {
                    return false;
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"netsh failed with exit code {process.ExitCode}");
                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");
                    return false;
                }

                bool noRuleFound = output.Contains("No rules match the specified criteria", StringComparison.OrdinalIgnoreCase);
                if (noRuleFound)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> AddFirewallRuleAsync(string ruleName, string programPath, string direction)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                startInfo.ArgumentList.Add("advfirewall");
                startInfo.ArgumentList.Add("firewall");
                startInfo.ArgumentList.Add("add");
                startInfo.ArgumentList.Add("rule");

                startInfo.ArgumentList.Add($"name={ruleName}");
                startInfo.ArgumentList.Add($"dir={direction}");
                startInfo.ArgumentList.Add("action=block");
                startInfo.ArgumentList.Add($"program={programPath}");
                startInfo.ArgumentList.Add("enable=yes");
                startInfo.ArgumentList.Add("profile=any");
                startInfo.ArgumentList.Add("protocol=any");

                using Process? process = Process.Start(startInfo);

                if (process == null)
                {
                    return false;
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"netsh failed with exit code {process.ExitCode}");
                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> DeleteFirewallRuleAsync(string ruleName)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                startInfo.ArgumentList.Add("advfirewall");
                startInfo.ArgumentList.Add("firewall");
                startInfo.ArgumentList.Add("delete");
                startInfo.ArgumentList.Add("rule");
                startInfo.ArgumentList.Add($"name={ruleName}");

                using Process? process = Process.Start(startInfo);

                if (process == null)
                {
                    return false;
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to delete rule: {ruleName}");
                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
