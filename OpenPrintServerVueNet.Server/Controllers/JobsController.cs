using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Server.Classes;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Payload;

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
        [HttpPost]
        public PaginationResults<Job> Get(GetResultsPayload resultsPayload)
        {
            var res = db.Jobs
                .Include(j => j.Printer)
                .OrderByDescending(j => j.Id);
            if (resultsPayload.Id != null && resultsPayload.Id > 0)
            {
                var printer = db.Printers.Find(resultsPayload.Id);
                if (printer != null)
                {
                    res = (IOrderedQueryable<Job>)res.Where(j => j.Printer == printer);
                }

            }
            return new PaginationResults<Job>(res, resultsPayload.Page, resultsPayload.Limit);
        }

        [HttpGet("{id}")]
        public IActionResult GetJob(Int64 id)
        {
            return Ok(db.Jobs
                .Include(j => j.Printer)
                .FirstOrDefault(j => j.Id == id));
        }

        [HttpGet("status/synced/{yn}")]
        public IActionResult GetPrintedJobs(string yn)
        {
            return Ok(db.Jobs.Include(i => i.Printer).Where(j => j.Synced == (yn == "y")));
        }
    }
}
