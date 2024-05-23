using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Server.Contexts;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        ApplicationContext db;

        public JobsController(ApplicationContext context)
        {
            db = context;
        }

        // GET: JobsController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> Get()
        {
            return await db.Jobs.ToListAsync();
        }
    }
}
