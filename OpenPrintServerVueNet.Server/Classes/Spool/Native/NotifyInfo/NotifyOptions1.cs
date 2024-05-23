﻿using System;
using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public struct NotifyOptions1 {
        public UInt16 F1_Type;
        public UInt16 F2_Reserved0;
        public UInt32 F3_Reserved1;
        public UInt32 F4_Reserved2;
        public UInt32 F5_Count;
        public IntPtr F6_Children;
    }

}
