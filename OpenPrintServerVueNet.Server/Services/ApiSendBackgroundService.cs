using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Server.Contexts;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenPrintServerVueNet.Server.Enums;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Classes.DTO;
using OpenPrintServerVueNet.Server.Payload;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;

namespace OpenPrintServerVueNet.Server.Services
{
    public class ApiSendBackgroundService : BackgroundService
    {
        private int limit = 50;

        private int timeout = 60; // one per minute

        private readonly IServiceProvider _serviceProvider;

        protected PrintServer? printServer = null;

        protected JsonSerializerOptions serializerOptions;

        public ApiSendBackgroundService(IServiceProvider provider)
        {
            _serviceProvider = provider;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        }

        /// <summary>
        /// Execute main task loop
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    StartApiRequests();
                });
                await Task.Delay(TimeSpan.FromSeconds(timeout));
            }
        }

        /// <summary>
        /// Start api requests function
        /// </summary>
        protected async void StartApiRequests()
        {
            var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            using (ApplicationContext db = context)
            {
                var apiEnabled = db.Config.FirstOrDefault(c => c.Key == ConfigEnum.ApiEnabled);
                if (apiEnabled != null)
                {
                    if (apiEnabled.Value != null && bool.Parse(apiEnabled.Value) == true)
                    {
                        var apiUrl = db.Config.FirstOrDefault(c => c.Key == ConfigEnum.ApiUrl);
                        if (apiUrl != null && apiUrl.Value != null)
                        {
                            var jobs = await db.Jobs
                                .Include(i => i.Printer).Include(p => p.Printer.Ports)
                                .Select(j => new
                                {
                                    j.Id,
                                    j.JobId,
                                    j.PrinterId,
                                    j.PrinterName,
                                    j.Status,
                                    j.StatusString,
                                    Printer = new PrinterDTO(j.Printer),
                                    j.Synced,
                                    j.PagesTotal,
                                    j.PagesPrinted,
                                    j.MachineName,
                                    j.Username,
                                    j.BytesTotal,
                                    j.BytesPrinted,
                                    j.Document,
                                    j.Printer_Copies,
                                    j.Printer_Palette,
                                    j.Printer_DefaultSource,
                                    j.Printer_Orientation,
                                    j.Printer_Paper_Size,
                                    j.Time,
                                    j.Submitted
                                })
                                .Where(j => j.Synced == false)
                                .OrderBy(j => j.Id)
                                .ThenBy(j => j.PrinterId)
                                .Take(limit)
                                .ToListAsync();
                            if (jobs.Any())
                            {
                                var opts = new JsonSerializerOptions();
                                opts.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                                var jsonContent = JsonSerializer.Serialize(jobs, opts);
                                var url = apiUrl.Value;
                                if (!url.EndsWith("/"))
                                {
                                    url += "/";
                                }
                                var client = new HttpClient()
                                {
                                    BaseAddress = new Uri(url)
                                };

                                try
                                {
                                    var res = await client.PostAsJsonAsync("jobs", jsonContent);
                                    if (res.IsSuccessStatusCode)
                                    {
                                        var result = res.Content.ReadAsStringAsync().Result;
                                        var payload = JsonSerializer.Deserialize<JobSyncedPayload>(result);
                                        if (payload != null && payload.ids.Count() > 0)
                                        {
                                            db.Jobs
                                                .Where(j => payload.ids.Contains(j.Id))
                                                .Where(j =>
                                                   j.Status != null
                                                   ? (
                                                   (PrintJobStatus)j.Status).HasFlag(
                                                       PrintJobStatus.Printed |
                                                       PrintJobStatus.Deleted
                                                   )
                                                   : j.Status > 0 // hotfix
                                                )
                                                .ExecuteUpdate(s => s.SetProperty(j => j.Synced, j => true));
                                            db.SaveChanges();
                                        }
                                        Console.WriteLine("\r\n==============\r\n{0} - {1}\r\n================\r\n", res.StatusCode, result);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\n==============\r\n{0} - {1}\r\n================\r\n", res.StatusCode, apiUrl.Value);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                };
                            }


                        }
                    }
                };

            }
        }



        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
