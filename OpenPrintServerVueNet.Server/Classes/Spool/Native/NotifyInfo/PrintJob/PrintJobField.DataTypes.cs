using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    public static partial class PrintJobField_DataTypes {

        public static NotifyInfoDataType DataType(this PrintJobField This) {
           switch(This) {
                case PrintJobField.Printer_Name: return NotifyInfoDataType.String;
                case PrintJobField.Machine_Name: return NotifyInfoDataType.String;
                case PrintJobField.User_Name: return NotifyInfoDataType.String;
                case PrintJobField.Notify_Name: return NotifyInfoDataType.String;
                case PrintJobField.DataType: return NotifyInfoDataType.String;
                case PrintJobField.PrintProcessor: return NotifyInfoDataType.String;
                case PrintJobField.Parameters: return NotifyInfoDataType.String;
                case PrintJobField.Driver_Name: return NotifyInfoDataType.String;
                case PrintJobField.StatusString: return NotifyInfoDataType.String;
                case PrintJobField.Document: return NotifyInfoDataType.String;

                case PrintJobField.Port_Name: return NotifyInfoDataType.StringCommaList;

                case PrintJobField.Status: return NotifyInfoDataType.JobStatus;

                case PrintJobField.Priority: return NotifyInfoDataType.Number;
                case PrintJobField.Position: return NotifyInfoDataType.Number;
                case PrintJobField.PagesTotal: return NotifyInfoDataType.Number;
                case PrintJobField.PagesPrinted: return NotifyInfoDataType.Number;
                case PrintJobField.BytesTotal: return NotifyInfoDataType.Number;
                case PrintJobField.BytesPrinted: return NotifyInfoDataType.Number;

                case PrintJobField.Submitted: return NotifyInfoDataType.DateTime;

                case PrintJobField.StartTime: return NotifyInfoDataType.Time;
                case PrintJobField.UntilTime: return NotifyInfoDataType.Time;

                case PrintJobField.Time: return NotifyInfoDataType.Duration;

                //Yes, the seucirty descriptor is not supported for jobs per https://docs.microsoft.com/en-us/windows/desktop/printdocs/printer-notify-info-data
                case PrintJobField.SecurityDescriptor: return NotifyInfoDataType.NotSupported;

                case PrintJobField.DevMode: return NotifyInfoDataType.DevMode;

                default: return NotifyInfoDataType.NotImplemented;
            };

        }

    }
}
