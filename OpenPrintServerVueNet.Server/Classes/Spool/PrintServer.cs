using Newtonsoft.Json.Linq;
using OpenPrintServerVueNet.Classes.DTO;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using OpenPrintServerVueNet.Server.Classes.DTO;
using OpenPrintServerVueNet.Server.Helpers;
using OpenPrintServerVueNet.Server.Models;
using System.Text.Json;
using System.Text.Json.Serialization;


using System.Threading.Channels;

namespace OpenPrintServerVueNet.Classes.Spool
{
    public class PrintServer
    {

        public PrintWatcher? watcher = null;

        public delegate void JobHandler(PrintJobDTO job);

        public event JobHandler? OnJobReiceved;

        public delegate void PrinterHandler(KeyValuePair<uint, PrintDeviceData> printer);

        public event PrinterHandler? OnPrinterChanged;

        public delegate void PrinterAddHandler(KeyValuePair<uint, PrintDeviceData> printer);

        public event PrinterAddHandler? OnPrinterAdd;

        public delegate void PrinterDeleteHandler(KeyValuePair<uint, PrintDeviceData> printer);

        public event PrinterDeleteHandler? OnPrinterDelete;

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
                    PrintDeviceEvents.Job_Delete,
                    PrintDeviceEvents.Printer,
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
                    PrintDeviceField.Status,
                    PrintDeviceField.Printer_Name,
                    PrintDeviceField.Share_Name,
                    PrintDeviceField.Port_Name,
                    PrintDeviceField.Comment,
                    PrintDeviceField.Driver_Name,
                    PrintDeviceField.Location,
                    PrintDeviceField.Server_Name,
                    PrintDeviceField.JobsQueued
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
            }
            catch { }

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
            var list = new List<PrinterChangedDTO>();

            Console.WriteLine($"\r\n============TRIGGERED: {e.Cause.ToString()} (Discarded: {e.Discarded})\r\n");


            if (e.Cause.HasFlag(PrintDeviceEvents.Printer_Add))
            {
                foreach (var Device in e.PrintDevices)
                {
                    Console.WriteLine("Add printer {0}", Device.Value.PrintDevice_PrinterName().Value.ToString());
                    OnPrinterAdd?.Invoke(Device);
                }
            }
            if (e.Cause.HasFlag(PrintDeviceEvents.Printer_Delete))
            {
                foreach (var Device in e.PrintDevices)
                {
                    if (Device.Value.PrintDevice_Status().Value.HasFlag(PrintDeviceStatus.Pending_Deletion))
                    {
                        Console.WriteLine("Delete printer {0}", Device.Value.PrintDevice_PrinterName().Value.ToString());
                        OnPrinterDelete?.Invoke(Device);
                    }
                }
            }
            if (e.Cause.HasFlag(PrintDeviceEvents.Printer_Set))
            {
                foreach (var Device in e.PrintDevices)
                {
                    OnPrinterChanged?.Invoke(Device);
                }
            }

            foreach (var Job in e.PrintJobs)
            {
                var job = new PrintJobDTO(Job.Key, Job.Value);
                OnJobReiceved?.Invoke(job);
                Console.WriteLine($@" Print Job #{Job.Key}");
            }
            //    case PrintDeviceEvents.Job_Add:
            //     case PrintDeviceEvents.Job_Delete:



            /* foreach (var Job in e.PrintJobs)
             {
                 var job = new PrintJobDTO(Job.Key, Job.Value);
                 OnJobReiceved?.Invoke(job);
                 Console.WriteLine($@" Print Job #{Job.Key}");
             }*/



            //  Console.WriteLine();
        }
    }
}
