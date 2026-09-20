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

        public ScanController(IExecutableScanner executableScanner)
        {
            _executableScanner = executableScanner;
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
