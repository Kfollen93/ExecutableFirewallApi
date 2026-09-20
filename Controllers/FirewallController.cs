using ExecutableFirewallApi.Models.Enums;
using ExecutableFirewallApi.Models.Requests;
using ExecutableFirewallApi.Models.Responses;
using ExecutableFirewallApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExecutableFirewallApi.Controllers
{
    [ApiController]
    [Route("api/firewall")]
    public class FirewallController : ControllerBase
    {
        private readonly IExecutableScanner _scanner;
        private readonly IFirewall _firewall;

        public FirewallController(IExecutableScanner scanner, IFirewall firewall)
        {
            _scanner = scanner;
            _firewall = firewall;
        }

        [HttpPost]
        public async Task<ActionResult<BlockResult>> BlockAsync(FolderRequest request)
        {
            var executables = _scanner.Scan(request.FolderPath);
            int newlyBlocked = 0;
            int alreadyBlocked = 0;
            int failed = 0;

            foreach (var exe in executables)
            {
                FirewallBlockOutcome result = await _firewall.BlockAsync(exe.FullPath);

                switch (result)
                {
                    case FirewallBlockOutcome.Blocked:
                        newlyBlocked++;
                        break;

                    case FirewallBlockOutcome.AlreadyBlocked:
                        alreadyBlocked++;
                        break;

                    case FirewallBlockOutcome.Failed:
                        failed++;
                        break;
                }
            }

            var response = new BlockResult(executables.Count, newlyBlocked, alreadyBlocked, failed);
            return Ok(response);
        }
    }
}
