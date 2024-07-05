using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;

namespace OpenPrintServerVueNet.Server.Classes.DTO
{
    public class PrinterChangedDTO
    {
        public string? Name { get; set; }
        public string? SharedName { get; set; }
        public string? Comment { get; set; }
        public string? DriverName { get; set; }
        public string? Location { get; set; }
        public PrintDeviceStatus? Status { get; set; }
        public UInt16 JobsQueued { get; set; }
    }
}
