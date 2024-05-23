﻿using System;
using System.Runtime.InteropServices;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public struct NotifyInfoHeader {
        public uint Version;
        public NotifyInfoFlags Flags;
        public uint Count;
    }

    [StructLayout(layoutKind: Defaults.LayoutKindDefault, CharSet = Defaults.CharSetDefault)]
    public struct NotifyInfo {
        public NotifyInfoHeader Header;

        public NotifyInfoData[] Data;

        public static NotifyInfo From(IntPtr Handle) {
            var Parsed = Marshal.PtrToStructure<NotifyInfoHeader>(Handle);

            var ret = new NotifyInfo() {
                Header = Parsed,
            };

            var TypePointer = Handle + (int) Marshal.OffsetOf<NotifyInfo>(nameof(Data));

            ret.Data = Marshal2.PtrToArray<NotifyInfoData>(TypePointer, ret.Header.Count);

            return ret;
        }
    }

    

}
