using ExecutableFirewallApi.Models.Requests;
using ExecutableFirewallApi.Models.Responses;
using ExecutableFirewallApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExecutableFirewallApi.Controllers
{
    [ApiController]
    [Route("api/scan")]
    public class ScanController : ControllerBase
    {
        private readonly IExecutableScanner _executableScanner;
        private readonly IFirewall _firewall;

        public ScanController(IExecutableScanner executableScanner, IFirewall firewall)
        {
            _executableScanner = executableScanner;
            _firewall = firewall;
        }

        [HttpPost]
        public async Task<ActionResult<ScanResult>> Scan(FolderRequest request)
        {
            var executables = _executableScanner.Scan(request.FolderPath);
            var result = new ScanResult(
                request.FolderPath,
                executables.Count,
                executables);

            return Ok(result);
        }
    }
}
