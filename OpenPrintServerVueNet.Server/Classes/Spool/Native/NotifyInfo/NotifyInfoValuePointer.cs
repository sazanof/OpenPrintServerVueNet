using System;
using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public struct NotifyInfoValuePointer {
        public uint Size;
        public IntPtr Address;

    }


}
