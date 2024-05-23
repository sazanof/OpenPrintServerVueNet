﻿using System;
using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public class NotifyOptions {
        public uint Version;
        public NotifyOptionsFlags Flags;
        public uint Count;
        public IntPtr Children;

        public NotifyOptions() {
            Version = 2;
            Flags = 0;
            Count = 0;
            Children = IntPtr.Zero;
        }
    }

}
