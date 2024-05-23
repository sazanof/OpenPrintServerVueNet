using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using System.Collections.Generic;

namespace OpenPrintServerVueNet.Classes.Spool {

    public class PrintWatcherStartArgs {
        public bool GetAllFieldsOnChange { get; set; }

        public string PrintDeviceName { get; set; }

        public List<PrintDeviceEvents> PrintDeviceEvents { get; private set; } = new List<PrintDeviceEvents>();
        public PrintDeviceHardwareType PrintDeviceHardwareType { get; set; }
        public List<PrintDeviceField> PrintDeviceFields { get; private set; } = new List<PrintDeviceField>();


        public List<PrintJobField> PrintJobFields { get; private set; } = new List<PrintJobField>();


    }

}
