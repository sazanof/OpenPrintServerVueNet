using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: LayoutKind.Explicit, CharSet = Defaults.CharSetDefault)]
    public struct NotifyInfoValue {
        [FieldOffset(0)]
        public NotifyInfoValueNumeric NumericData;

        [FieldOffset(0)]
        public NotifyInfoValuePointer PointerData;
    }


}
