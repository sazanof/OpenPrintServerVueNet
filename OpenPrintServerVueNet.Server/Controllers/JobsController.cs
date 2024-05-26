using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Server.Classes;
using OpenPrintServerVueNet.Server.Contexts;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
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
        [HttpGet("page/{page}")]
        public PaginationResults<Job> Get(int page)
        {
            return new PaginationResults<Job>(db.Jobs
                .Include(j => j.Printer)
                .OrderByDescending(j => j.Id), page, 50);
        }

        [HttpGet("{id}")]
        public IActionResult GetJob(Int64 id)
        {
            return Ok(db.Jobs
                .Include(j => j.Printer)
                .FirstOrDefault(j => j.Id == id));
        }
    }
}
