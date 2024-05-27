using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Payload;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/config")]
    public class ConfigController : Controller
    {
        private readonly ApplicationContext _db;

        public ConfigController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            return Ok(_db.Config);
        }

        [HttpPost]
        public IActionResult SetConfig(List<ConfigPayload> configPayload)
        {
            foreach(ConfigPayload payload in configPayload)
            {
                var c = new Config()
                {
                    Key = payload.Key,
                    Value = payload.Value
                };
                var existing = _db.Config.FirstOrDefault(conf => conf.Key == c.Key);
                if (existing != null)
                {
                    existing.Value = c.Value;
                }
                else
                {
                    _db.Config.Add(c);
                }
            }
            
            _db.SaveChanges();
            return Ok(_db.Config);
        }

    }
}
