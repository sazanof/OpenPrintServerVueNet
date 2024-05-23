using OpenPrintServerVueNet.Classes.DTO;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using System.Text.Json;
using System.Threading.Channels;

namespace OpenPrintServerVueNet.Classes.Spool
{
    public class PrintServer
    {

        public PrintWatcher? watcher = null;

        public delegate void JobHandler(PrintJobDTO job);
       
        public event JobHandler? OnJobReiceved;

        public void Start()
        {
            watcher = PrintWatcher.Start(new PrintWatcherStartArgs()
            {
                GetAllFieldsOnChange = true,
                PrintDeviceHardwareType = PrintDeviceHardwareType.All,

                PrintDeviceEvents = { 
                    //PrintDeviceEvents.Job,
                    //PrintDeviceEvents.Job_Add,
                    PrintDeviceEvents.Job_Add,
                    PrintDeviceEvents.Job_Delete
                },

                PrintJobFields = {
                    PrintJobField.Printer_Name,
                    PrintJobField.DevMode,
                    PrintJobField.Document,
                    PrintJobField.User_Name,
                    PrintJobField.Driver_Name,
                    PrintJobField.Machine_Name,
                    PrintJobField.Notify_Name,
                    PrintJobField.PagesPrinted,
                    PrintJobField.PagesTotal,
                    PrintJobField.Port_Name,
                    PrintJobField.BytesTotal,
                    PrintJobField.BytesPrinted,
                    PrintJobField.Position,
                    PrintJobField.Priority,
                    PrintJobField.Status,
                    PrintJobField.StatusString,
                    PrintJobField.Submitted,
                    PrintJobField.Time,
                },
                PrintDeviceFields = {
                    PrintDeviceField.Printer_Name,
                    PrintDeviceField.Share_Name,
                    PrintDeviceField.Port_Name,
                    PrintDeviceField.Comment
                }

            });
            var Runner = Task.Run(() => ShowEvents(watcher.Events));

           // Console.WriteLine("Press any key to stop the watcher");
           // Console.ReadLine();
//
            //Console.WriteLine("Press any key to exit");
          //  Console.ReadLine();
        }

        public void Stop()
        {
            try
            {
                if (watcher != null)
                {
                    watcher.Dispose();
                }
            } catch { }
            
        }


        private void ShowRecords(IEnumerable<Native.IRecord> Records)
        {
            foreach (var item in Records)
            {

                if (item.Value is DevModeA DMA)
                {
                    foreach (var DMR in DMA.AllRecords())
                    {
                        Console.WriteLine($@"      {DMR.Name} = {DMR.Value}");
                    }
                }

                else if (item.Value is System.Collections.ObjectModel.ReadOnlyCollection<string> collection)
                {
                    Console.WriteLine($@"{string.Join(",", collection)}");
                }
                else
                {
                    Console.WriteLine($@"    {item.Name} = {item.Value}");
                }
            }
        }


        private async Task ShowEvents(ChannelReader<PrintWatcherEventArgs> Events)
        {
            //LoggerManager.GetInstance().LogWarning("Task ShowEvents started");
            while (await Events.WaitToReadAsync())
            {
                while (Events.TryRead(out var Item))
                {
                    ShowEvents(Item);
                }
            }


        }

        private void ShowEvents(PrintWatcherEventArgs e)
        {
           // Console.WriteLine($@"TRIGGERED: {e.Cause} (Discarded: {e.Discarded})");
           /* foreach (var Device in e.PrintDevices)
            {
                Console.WriteLine($@"  Print Device #{Device.Key}");
                ShowRecords(Device.Value.PrintDevice_Records.Values);
                Console.WriteLine();
            }*/

            foreach (var Job in e.PrintJobs)
            {
              
                var job = new PrintJobDTO(Job.Key, Job.Value);
                OnJobReiceved?.Invoke(job);
                Console.WriteLine($@"  Print Job #{Job.Key}");
                var opts = new JsonSerializerOptions();
                opts.WriteIndented = true;
                Console.WriteLine(JsonSerializer.Serialize(job, opts));

                // ShowRecords(Job.Value.PrintJob_Records.Values);
                //      Console.WriteLine();
            }

          //  Console.WriteLine();
        }
    }
}
