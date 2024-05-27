﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://learn.microsoft.com/en-us/dotnet/api/system.printing.printjobstatus?view=windowsdesktop-8.0
namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    [Flags]
    public enum PrintJobStatus {
        Paused = 1,
        Error = 2,
        Deleting = 4,
        Spooling = 8,
        Printing = 16,
        Offline = 32,
        OutOfPaper = 0x40,
        Printed = 0x80,
        Deleted = 0x100,
        Blocked_DevQ = 0x200,
        UserInterventionRequired = 0x400,
    }
}
