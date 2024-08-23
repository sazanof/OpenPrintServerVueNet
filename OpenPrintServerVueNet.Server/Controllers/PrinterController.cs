using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Classes;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Hubs;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Responses;
using OpenPrintServerVueNet.Server.Services;
using System.Data.Common;
using System.Management;
using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/printers")]
    public class PrinterController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IHubContext<NotificationsHub> _notificationsHub;
        private readonly UserManager<User> _userManager;

        public Snmp? snmp = null;

        public static Task? syncTask;
        public PrinterController(ApplicationContext db, IHubContext<NotificationsHub> notificationsHub, UserManager<User> userManager)
        {
            _db = db;
            _notificationsHub = notificationsHub;
            _userManager = userManager;

        }

        [HttpGet("sync/status")]
        public async Task<IActionResult> GetSyncStatus()
        {
            return await Task.Run(async () =>
            {
                var user = await _userManager.GetUserAsync(User);
                var response = new SyncResponse();
                //Send current status
                response.Status = SyncPrintersBackgroundService.Status;
                await _notificationsHub.Clients.User(user?.Id).SendAsync("on.printers.sync.status", JsonSerializer.Serialize(response));
                return Ok(response);

            });
        }

        /// <summary>
        /// Sync all or one printer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("sync/{id?}")]
        public async Task<IActionResult> SyncPrinters(int id = 0)
        {
            var response = new SyncResponse();

            // Check if task is already running
            if (SyncPrintersBackgroundService.Status == SyncStatusEnum.Running)
            {
                response.Status = SyncStatusEnum.Duplicated;
                return Ok(response);
            }
            // Start the task
            SyncPrintersBackgroundService.SyncById(id);
            response.Status = SyncStatusEnum.Running;
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetPrintersList()
        {

            return await Task.Run(() =>
            {
                return Ok(_db.Printers.Include(x => x.Ports).Include(c => c.Consumables));
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrinter(int id)
        {
            return await Task.Run(() =>
            {
                return Ok(
                    _db.Printers
                    .Include(x => x.Ports)
                    .Include(c => c.Consumables)
                    .FirstOrDefault(p => p.Id == id)
                    );
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePrinter(int id)
        {
            var printer = _db.Printers.FirstOrDefault(p => p.Id == id);
            _db.Remove<Printer>(printer);
            _db.SaveChanges();
            return Ok();
        }


    }
}
