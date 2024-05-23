using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    public static partial class PrintDeviceField_DataTypes {

        public static NotifyInfoDataType DataType(this PrintDeviceField This) {
            switch(This) {
                case PrintDeviceField.Printer_Name: return NotifyInfoDataType.String;
                case PrintDeviceField.Share_Name: return NotifyInfoDataType.String;
                case PrintDeviceField.Driver_Name: return NotifyInfoDataType.String;
                case PrintDeviceField.Comment: return NotifyInfoDataType.String;
                case PrintDeviceField.Location: return NotifyInfoDataType.String;
                case PrintDeviceField.SeparatorFile: return NotifyInfoDataType.String;
                case PrintDeviceField.PrintProcessor: return NotifyInfoDataType.String;
                case PrintDeviceField.Parameters: return NotifyInfoDataType.String;
                case PrintDeviceField.DataType: return NotifyInfoDataType.String;

                case PrintDeviceField.Port_Name: return NotifyInfoDataType.StringCommaList;

                case PrintDeviceField.Server_Name: return NotifyInfoDataType.NotSupported;
                case PrintDeviceField.Pages_Total: return NotifyInfoDataType.NotSupported;
                case PrintDeviceField.Pages_Printed: return NotifyInfoDataType.NotSupported;
                case PrintDeviceField.Bytes_Total: return NotifyInfoDataType.NotSupported;
                case PrintDeviceField.Bytes_Printed: return NotifyInfoDataType.NotSupported;
                case PrintDeviceField.StatusString: return NotifyInfoDataType.NotSupported;

                case PrintDeviceField.SecurityDescriptor: return NotifyInfoDataType.SecurityDescriptor;


                case PrintDeviceField.Priority: return NotifyInfoDataType.Number;
                case PrintDeviceField.PriorityDefault: return NotifyInfoDataType.Number;
                case PrintDeviceField.JobsQueued: return NotifyInfoDataType.Number;
                case PrintDeviceField.Pages_AveragePerMinute: return NotifyInfoDataType.Number;

                case PrintDeviceField.StartTime: return NotifyInfoDataType.Time;
                case PrintDeviceField.UntilTime: return NotifyInfoDataType.Time;

                case PrintDeviceField.Attributes: return NotifyInfoDataType.PrinterAttributes;
                case PrintDeviceField.Status: return NotifyInfoDataType.PrinterStatus;

                case PrintDeviceField.DevMode: return NotifyInfoDataType.DevMode;

                default: return NotifyInfoDataType.NotImplemented;
            };
        }

    }
}


