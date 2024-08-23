using OpenPrintServerVueNet.Models;
using OpenPrintServerVueNet.Classes.DTO;
using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Server.Contexts;
using System.Text.Json;
using OpenPrintServerVueNet.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;
using OpenPrintServerVueNet.Server.Helpers;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace OpenPrintServerVueNet.Server.Services
{
    public class JobBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<NotificationsHub> _notificationsHub;

        protected PrintServer? printServer = null;

        protected JsonSerializerOptions serializerOptions;

        public JobBackgroundService(IServiceProvider provider, IHubContext<NotificationsHub> notificationsHub)
        {
            _serviceProvider = provider;
            _notificationsHub = notificationsHub;

            serializerOptions = new JsonSerializerOptions();
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    StartApiRequests();
                });
            }
        }

        protected void StartApiRequests()
        {
            if (printServer == null)
            {
                printServer = new PrintServer();
                printServer.OnJobReiceved += PrintServer_OnJobReiceved;
                printServer.OnPrinterChanged += PrintServer_OnPrinterChanged;
                printServer.OnPrinterAdd += PrintServer_OnPrinterAdd;
                printServer.OnPrinterDelete += PrintServer_OnPrinterDelete;
                printServer.Start();
            }
        }

        /// <summary>
        /// Delete printer hook
        /// </summary>
        /// <param name="printer"></param>
        private async void PrintServer_OnPrinterDelete(KeyValuePair<uint, PrintDeviceData> printer)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            
            using (ApplicationContext db = context)
            {
                if (!printer.Value.PrintDevice_Status().Value.Equals(OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo.PrintDeviceStatus.None))
                {
                    //if (printer.Value.PrintDevice_Status().Value.HasFlag(OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo.PrintDeviceStatus.Pending_Deletion)) { }
                    if (printer.Value.PrintDevice_Status().Value.HasFlag(OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo.PrintDeviceStatus.Pending_Deletion)) { }
                    {
                        Console.WriteLine(
                            "Triggered PrintServer_OnPrinterDelete, {0}, {1}",
                            printer.Value.PrintDevice_PrinterName().Value, printer.Value.PrintDevice_Status().Value.ToString()
                            );
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                var _printer = PrinterHelper.DeletePrinter(printer.Value, db, true);
                                
                                dbContextTransaction.Commit();

                                await _notificationsHub.Clients.All.SendAsync("on.printer.delete", JsonSerializer.Serialize(_printer, serializerOptions));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }


            }
        }
        /// <summary>
        /// Add printer hook
        /// </summary>
        /// <param name="printer"></param>
        private async void PrintServer_OnPrinterAdd(KeyValuePair<uint, PrintDeviceData> printer)
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            using (ApplicationContext db = context)
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var p = PrinterHelper.AddPrinter(printer.Value, db, true);
                        if (p != null) {
                            PrinterHelper.AddOrEditPrinterPort(p, db, true);
                            PrinterHelper.SyncSnmpValues(p, db);

                            db.SaveChanges();

                            dbContextTransaction.Commit();

                            await _notificationsHub.Clients.All.SendAsync("on.printer.add", JsonSerializer.Serialize(p, serializerOptions));
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printer"></param>
        private async void PrintServer_OnPrinterChanged(KeyValuePair<uint, PrintDeviceData> printer)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                var name = printer.Value.PrintDevice_PrinterName().Value;

                Console.WriteLine("Triggered by background printer {0}", printer.Value.PrintDevice_PrinterName().Value);
                Console.WriteLine(JsonSerializer.Serialize(printer));

                using (ApplicationContext db = context)
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var founded = db.Printers.Include(p=>p.Ports).Include(p=>p.Consumables).FirstOrDefault(p => p.Name == name);
                            if (founded != null)
                            {
                                founded.Name = name;
                                founded.PrinterStatus = (ushort?)printer.Value.PrintDevice_Status().Value;
                                founded.Location = printer.Value.PrintDevice_Location().Value;
                                founded.ShareName = printer.Value.PrintDevice_ShareName().Value;
                                founded.DriverName = printer.Value.PrintDevice_DriverName().Value;
                                founded.Comment = printer.Value.PrintDevice_Comment().Value;

                                db.SaveChanges();
                                dbContextTransaction.Commit();

                                await _notificationsHub.Clients.All.SendAsync("on.printer.changed", JsonSerializer.Serialize(founded, serializerOptions));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
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

                    var s = job.Submitted;
                    var _submitted = DateTime.SpecifyKind(s, DateTimeKind.Utc);

                    var _submittedModify = s.ToLocalTime();// !!!!!!!

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
                        Submitted = _submittedModify,
                        Time = job.Time.ToString(),
                        Priority = (int)job.Priority,
                        Position = (int)job.Position,
                        PagesPrinted = (int)job.PagesPrinted,
                        PagesTotal = (int)job.PagesTotal,
                        BytesPrinted = (int)job.BytesPrinted,
                        BytesTotal = (int)job.BytesTotal,
                        Synced = false,
                    };

                    if (model.PagesTotal > 0) // Add only jobs with pages > 0
                    {
                        var printer = db.Printers.FirstOrDefault(p => p.Name == job.PrinterName && p.DriverName == job.DriverName);
                        if (printer != null)
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

                        await _notificationsHub.Clients.All.SendAsync("on.job", JsonSerializer.Serialize(founded != null ? founded : model, serializerOptions));
                    }


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
