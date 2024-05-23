using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public struct NotifyInfoValueNumeric {
        public uint Value1;
        public uint Value2;
    }


}
