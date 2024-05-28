using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Enums;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/api/snmp/variables")]
    public class SnmpVariablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
