using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.DevMode {
    [StructLayout(layoutKind: LayoutKind.Explicit, CharSet = Defaults.CharSetDefault)]
    public struct DevMode_Device_Flags {
        [FieldOffset(0)]
        public DisplayFlags Display_Flags;

        [FieldOffset(0)]
        public PrinterPageLayout Printer_PageLayout;

    }

}
