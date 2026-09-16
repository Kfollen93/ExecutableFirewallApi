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
    }
}
