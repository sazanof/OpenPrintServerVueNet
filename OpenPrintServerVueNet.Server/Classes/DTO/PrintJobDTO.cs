using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;

namespace OpenPrintServerVueNet.Classes.DTO
{
    public class PrintJobDTO
    {
        public uint Id { get; set; }

        public string PrinterName { get; set; }

        public string MachineName { get; set; }

        public IReadOnlyCollection<string> Ports { get; set; }

        public string Username { get; set; }

        public string NotifyName { get; set; }

        public string DriverName { get; set; }

        public DevModeDTO DevMode { get; set; }

        public PrintJobStatus Status { get; set; }

        public string StatusString { get; set; }

        public string Document { get; set; }

        public uint Priority { get; set; }

        public uint Position { get; set; }

        public DateTime Submitted { get; set; }

        public TimeSpan Time { get; set; }

        public uint PagesTotal { get; set; }

        public uint PagesPrinted { get; set; }

        public uint BytesTotal { get; set; }

        public uint BytesPrinted { get; set; }

        public PrintJobDTO(uint JobId, PrintJobData JobData)
        {
            Id = JobId;
            PrinterName = JobData?.PrintJob_PrinterName().Value;
            MachineName = JobData?.PrintJob_MachineName().Value;
            // Ports
            Ports = JobData.PrintJob_PortName().Value;
            Username = JobData?.PrintJob_UserName().Value;
            NotifyName = JobData?.PrintJob_NotifyName().Value;
            DriverName = JobData?.PrintJob_DriverName().Value;
            //DevMode
            Status = JobData.PrintJob_Status().Value;
            StatusString = JobData.PrintJob_StatusString().Value;
            Document = JobData.PrintJob_Document().Value;
            Priority = JobData.PrintJob_Priority().Value;
            Position = JobData.PrintJob_Position().Value;
            Submitted = JobData.PrintJob_Submitted().Value;
            Time = JobData.PrintJob_Time().Value;
            PagesTotal = JobData.PrintJob_PagesTotal().Value;
            PagesPrinted = JobData.PrintJob_PagesPrinted().Value;
            BytesTotal = JobData.PrintJob_BytesTotal().Value;
            BytesPrinted = JobData.PrintJob_BytesPrinted().Value;
            DevMode = new DevModeDTO()
            {
                Printer_Copies = JobData.DevMode_Printer_Copies()?.Value,
                Printer_DefaultSource = JobData.DevMode_Printer_DefaultSource()?.Value,
                Printer_FormName = JobData.DevMode_Printer_FormName()?.Value,
                Printer_Orientation = JobData.DevMode_Printer_Orientation()?.Value,
                Printer_Palette = JobData.DevMode_Printer_Palette()?.Value,
                Printer_Paper_Size = JobData.DevMode_Printer_Paper_Size()?.Value,
                Printer_PrintQuality_X = JobData.DevMode_Printer_PrintQuality_X()?.Value,
                Printer_PrintQuality_Y = JobData.DevMode_Printer_PrintQuality_Y()?.Value,
            };

            //Data = JobData;

            foreach (var item in JobData.AllRecords())
            {

                if (item.Value is DevModeA DMA)
                {

                    foreach (var DMR in DMA.AllRecords())
                    {
                        // Console.WriteLine($@"      {DMR.Name} = {DMR.Value}");
                    }
                }

                else if (item.Value is System.Collections.ObjectModel.ReadOnlyCollection<string> collection)
                {
                    //Console.WriteLine($@"{string.Join(",", collection)}");
                }
                else
                {
                    // Console.WriteLine($@"    {item.Name} = {item.Value}");
                }
            }
        }
    }
}
