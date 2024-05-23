using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using OpenPrintServerVueNet.Classes.Spool.Native.SystemTime;
using OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo;
using OpenPrintServerVueNet.Classes.Spool.Native.Security;

namespace OpenPrintServerVueNet.Classes.Spool.Native.NotifyInfo {
    public static partial class FieldDataParser {

        public static IEnumerable<IRecord> ToRecords(this IEnumerable<NotifyInfoData> This) {
            foreach (var item in This) {
                yield return item.ToRecord();
            }
        }

        public static IRecord ToRecord(this NotifyInfoData This) {
            var Type = This.F1_Type;

            IRecord ret;
            switch (Type)
            {
                case NotifyInfoFieldType.Printer:
                    return ParsePrintDevice(This);
                case NotifyInfoFieldType.Job:
                    return ParsePrintJob(This);
                default:
                    return default;
            }
        }

        private static PrintDeviceRecord ParsePrintDevice(NotifyInfoData Item) {
            var Field = (PrintDeviceField)Item.F2_Field;
            var DataType = Field.DataType();
            var ID = Item.F4_Id;
            var Reserved = Item.F3_Reserved;

            var Value = Item.ParseValue(DataType);

            switch (Value) {
                case string V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case IReadOnlyCollection<string> V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case uint V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case PrintDeviceStatus V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                       case PrintDeviceAttribute V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case SecurityDescriptor V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                        case DateTime V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case TimeSpan V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                case DevModeA V1:
                    return PrintDeviceRecord.Create(ID, Reserved, Field, V1);
                default:
                    return default;
            }
        }

        private static PrintJobRecord ParsePrintJob(NotifyInfoData Item) {
            var Field = (PrintJobField)Item.F2_Field;
            var DataType = Field.DataType();
            var ID = Item.F4_Id;
            var Reserved = Item.F3_Reserved;

            var Value = Item.ParseValue(DataType);

            switch (Value)
            {
                case string V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case IReadOnlyCollection<string> V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case uint V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case PrintJobStatus V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case SecurityDescriptor V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case DateTime V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case TimeSpan V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                case DevModeA V1:
                    return PrintJobRecord.Create(ID, Reserved, Field, V1);
                default:
                    return default;
            }
        }

        

        public static object ParseValue(this NotifyInfoData This, NotifyInfoDataType DataType) {
            switch (DataType)
            {
                case NotifyInfoDataType.None:
                    return $@"This field has no data type";
                case NotifyInfoDataType.NotSupported:
                    return $@"Windows does not support retreiving the value of this field";
                case NotifyInfoDataType.String:
                    return This.ParseString();
                case NotifyInfoDataType.StringCommaList:
                    return This.ParseStringCommaList();
                case NotifyInfoDataType.JobStatus:
                    return This.ParseEnum<PrintJobStatus>();
                case NotifyInfoDataType.PrinterStatus:
                    return This.ParseEnum<PrintDeviceStatus>();
                case NotifyInfoDataType.PrinterAttributes:
                    return This.ParseEnum<PrintDeviceAttribute>();
                case NotifyInfoDataType.SecurityDescriptor:
                    return This.ParseSecurityDescriptor();
                case NotifyInfoDataType.Number:
                    return This.ParseNumber();
                case NotifyInfoDataType.DateTime:
                    return This.ParseDateTime();
                case NotifyInfoDataType.Time:
                    return This.ParseTime();
                case NotifyInfoDataType.Duration:
                    return This.ParseDuration();
                case NotifyInfoDataType.DevMode:
                    return This.ParseDevMode();
                default:
                   return $@"{DataType} has not been implemented";
            }
        }

        private static string ParseString(this NotifyInfoData This) {
            var ret = "";
            if (This.F5_NotifyData.PointerData.Address != IntPtr.Zero) {
                ret = Marshal.PtrToStringAnsi(This.F5_NotifyData.PointerData.Address);
            }
            return ret;
        }

        private static IReadOnlyCollection<string> ParseStringCommaList(this NotifyInfoData This) {
            return new System.Collections.ObjectModel.ReadOnlyCollection<string>(This.ParseString().Split(','));
        }

        private static SecurityDescriptor ParseSecurityDescriptor(this NotifyInfoData This) {
            var ret = default(SecurityDescriptor);
            if (This.F5_NotifyData.PointerData.Address != IntPtr.Zero) {
                ret = Marshal.PtrToStructure<SecurityDescriptor>(This.F5_NotifyData.PointerData.Address);
            }
            return ret;
        }

        private static T ParseEnum<T>(this NotifyInfoData This) where T : struct {
            var ret = default(T);

            ret = (T)Enum.ToObject(typeof(T), This.F5_NotifyData.NumericData.Value1);

            return ret;
        }

        private static uint ParseNumber(this NotifyInfoData This) {

            var ret = This.F5_NotifyData.NumericData.Value1;

            return ret;
        }

        private static DateTime ParseDateTime(this NotifyInfoData This) {
            var ret = default(DateTime);
            if(This.F5_NotifyData.PointerData.Address != IntPtr.Zero) {
                var tret = Marshal.PtrToStructure<SystemTime.SystemTime>(This.F5_NotifyData.PointerData.Address);
                ret = new DateTime(tret.Year, tret.Month, tret.Day, tret.Hour, tret.Minute, tret.Second, tret.Milliseconds);
            }

            return ret;
        }

        private static DateTime ParseTime(this NotifyInfoData This) {
            var ret = default(DateTime).AddMinutes(This.F5_NotifyData.NumericData.Value1);

            return ret;
        }

        private static TimeSpan ParseDuration(this NotifyInfoData This) {
            var ret = TimeSpan.FromSeconds(This.F5_NotifyData.NumericData.Value1);

            return ret;
        }

        private static DevModeA ParseDevMode(this NotifyInfoData This) {
            var ret = default(DevModeA);
            if (This.F5_NotifyData.PointerData.Address != IntPtr.Zero) {
                ret = Marshal.PtrToStructure<DevModeA>(This.F5_NotifyData.PointerData.Address);
            }

            return ret;
        }

    }
}
