using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Classes.DTO;
using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Server.Contexts;
using System.Diagnostics;
using System.Text.Json;
using OpenPrintServerVueNet.Server.Hibs;
using System;
using Microsoft.AspNetCore.SignalR;

namespace OpenPrintServerVueNet.Server.Services
{
    public class JobBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<NotificationsHub> _notificationsHub;

        protected PrintServer? printServer = null;

        public JobBackgroundService(IServiceProvider provider, IHubContext<NotificationsHub> notificationsHub)
        {
            _serviceProvider = provider;
            _notificationsHub = notificationsHub;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    StartPrintServer();
                });
            }
        }

        protected void StartPrintServer()
        {
            if (printServer == null)
            {
                printServer = new PrintServer();
                printServer.OnJobReiceved += PrintServer_OnJobReiceved;
                printServer.Start();
            }
        }

        private async void PrintServer_OnJobReiceved(PrintJobDTO job)
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;

                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                using (ApplicationContext db = context)
                {
                    Console.WriteLine("Triggered by background job {0}", job.Id);

                    Job model = new Job()
                    {
                        JobId = job.Id,
                        PrinterName = job.PrinterName,
                        Username = job.Username,
                        MachineName = job.MachineName,
                        NotifyName = job.NotifyName,
                        DriverName = job.DriverName,
                        Printer_Copies = job.DevMode.Printer_Copies,
                        Printer_DefaultSource = job.DevMode.Printer_DefaultSource,
                        Printer_FormName = job.DevMode.Printer_FormName,
                        Printer_Orientation = (int?)job.DevMode.Printer_Orientation,
                        Printer_Palette = (int?)job.DevMode.Printer_Palette,
                        Printer_Paper_Size = (int?)job.DevMode.Printer_Paper_Size,
                        Printer_PrintQuality_X = job.DevMode.Printer_PrintQuality_X,
                        Printer_PrintQuality_Y = job.DevMode.Printer_PrintQuality_Y,
                        Status = (int)job.Status,
                        StatusString = job.StatusString,
                        Document = job.Document,
                        Submitted = job.Submitted,
                        Time = job.Time.ToString(),
                        Priority = (int)job.Priority,
                        Position = (int)job.Position,
                        PagesPrinted = (int)job.PagesPrinted,
                        PagesTotal = (int)job.PagesTotal,
                        BytesPrinted = (int)job.BytesPrinted,
                        BytesTotal = (int)job.BytesTotal,
                        Synced = false,
                    };

                    var printer = db.Printers.FirstOrDefault(p => p.Name == job.PrinterName && p.DriverName == job.DriverName);
                    if(printer != null)
                    {
                        model.Printer = printer;
                    }

                    var submitted = model.Submitted.Value;

                    var founded = db.Jobs.ToList().FirstOrDefault(
                        j => j.Submitted == submitted &&
                       j.Document == model.Document &&
                       j.Username == model.Username &&
                       j.JobId == model.JobId
                        );
                    if (founded == null && printer != null)
                    {
                        db.Jobs.Add(model);
                    }
                    else
                    {
                        founded.Status = (int)job.Status;
                        founded.StatusString = job.StatusString;
                        founded.Time = job.Time.ToString();
                        founded.PagesTotal = (int)job.PagesTotal;
                        founded.PagesPrinted = (int)job.PagesPrinted;
                        founded.BytesTotal = (int)job.BytesTotal;
                        founded.BytesPrinted = (int)job.BytesPrinted;
                    }

                    db.SaveChanges();
                    await _notificationsHub.Clients.All.SendAsync("on.job", JsonSerializer.Serialize(founded != null ? founded : model));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public override void Dispose()
        {
            if (printServer != null)
            {
                printServer.Stop();
                printServer = null;
            }
            base.Dispose();
        }
    }
}
