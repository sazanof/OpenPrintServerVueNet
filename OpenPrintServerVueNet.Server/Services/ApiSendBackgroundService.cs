using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Server.Contexts;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenPrintServerVueNet.Server.Enums;
using Microsoft.EntityFrameworkCore;

namespace OpenPrintServerVueNet.Server.Services
{
    public class ApiSendBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;

        protected PrintServer? printServer = null;

        protected JsonSerializerOptions serializerOptions;

        public ApiSendBackgroundService(IServiceProvider provider)
        {
            _serviceProvider = provider;
            serializerOptions = new JsonSerializerOptions();
            serializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    StartApiRequests();
                });
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

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
                           // Console.WriteLine("StartApiRequests {0}", apiUrl.Value);
                            var jobs = db.Jobs.Include(i => i.Printer).Where(j => j.Synced == false).ToListAsync();
                            var opts = new JsonSerializerOptions();
                            opts.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                            var jsonContent = JsonSerializer.Serialize(jobs, opts);
                            var client = new HttpClient()
                            {
                                BaseAddress = new Uri(apiUrl.Value)
                            };

                            try
                            {
                               var res = await client.PostAsJsonAsync("jobs", jsonContent);
                                if (res.IsSuccessStatusCode)
                                {
                                    var result = res.Content.ReadAsStringAsync().Result;
                                    Console.WriteLine("\r\n==============\r\n{0} - {1}\r\n================\r\n", res.StatusCode, result);
                                } else
                                {
                                    Console.WriteLine("\r\n==============\r\n{0} - {1}\r\n================\r\n", res.StatusCode, "Unsuccessful request");
                                }
                            } catch(Exception ex)
                            {
                               Console.WriteLine(ex.ToString());
                            };
                           
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
