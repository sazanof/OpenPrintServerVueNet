﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    public enum NotifyInfoDataType {
        None,
        NotSupported,
        String,
        StringCommaList,
        PrinterStatus,
        PrinterAttributes,
        JobStatus,
        SecurityDescriptor,
        Number,
        DateTime,
        Time,
        Duration,
        DevMode,
        NotImplemented,
    }
}
