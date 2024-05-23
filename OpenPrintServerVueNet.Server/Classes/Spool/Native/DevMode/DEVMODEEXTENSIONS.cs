using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenPrintServerVueNet.Classes.Spool.Native.DevMode
{
    public static class DEVMODEEXTENSIONS
    {

        private static DevModeRecord GetField(this DevModeA This, DevModeField Name)
        {
            switch (Name)
            {
                case DevModeField.Panning_Height:
                    return DevModeRecord.Create(Name, This.Panning_Height);
                case DevModeField.Panning_Width:
                    return DevModeRecord.Create(Name, This.Panning_Width);
                case DevModeField.Dither:
                    return DevModeRecord.Create(Name, This.Dither);
                case DevModeField.Display_BitsPerPixel:
                    return DevModeRecord.Create(Name, This.Display_BitsPerPixel);
                case DevModeField.Display_PixelsPerLogicalInch:
                    return DevModeRecord.Create(Name, This.Display_PixelsPerLogicalInch);
                case DevModeField.Display_PixelsH:
                    return DevModeRecord.Create(Name, This.Display_PixelsH);
                case DevModeField.Display_PixelsW:
                    return DevModeRecord.Create(Name, This.Display_PixelsW);
                case DevModeField.Display_Position:
                    return DevModeRecord.Create(Name, This.Device.Display.Position);
                case DevModeField.Display_FixedOutput:
                    return DevModeRecord.Create(Name, This.Device.Display.DisplayFixedOutput);
                case DevModeField.Display_Flags:
                    return DevModeRecord.Create(Name, This.DeviceFlags.Display_Flags);
                case DevModeField.Display_Frequency:
                    return DevModeRecord.Create(Name, This.Display_Frequency);
                case DevModeField.Display_Orientation:
                    return DevModeRecord.Create(Name, This.Device.Display.Orientation);
                case DevModeField.Printer_Collate:
                    return DevModeRecord.Create(Name, This.Printer_Collate);
                case DevModeField.Printer_Copies:
                    return DevModeRecord.Create(Name, This.Device.Printer.Copies);
                case DevModeField.Printer_DefaultSource:
                    return DevModeRecord.Create(Name, This.Device.Printer.DefaultSource);
                case DevModeField.Printer_Duplex:
                    return DevModeRecord.Create(Name, This.Printer_Duplex);
                case DevModeField.Printer_FormName:
                    return DevModeRecord.Create(Name, This.Printer_FormName);
                case DevModeField.Printer_ICM_Intent:
                    return DevModeRecord.Create(Name, This.Printer_ICM_Intent);
                case DevModeField.Printer_ICM_Method:
                    return DevModeRecord.Create(Name, This.Printer_ICM_Method);
                case DevModeField.Printer_MediaType:
                    return DevModeRecord.Create(Name, This.Printer_MediaType);
                case DevModeField.Printer_Orientation:
                    return DevModeRecord.Create(Name, This.Device.Printer.Orientation);
                case DevModeField.Printer_PageLayout:
                    return DevModeRecord.Create(Name, This.DeviceFlags.Printer_PageLayout);
                case DevModeField.Printer_Palette:
                    return DevModeRecord.Create(Name, This.Printer_Color);
                case DevModeField.Printer_Paper_Length:
                    return DevModeRecord.Create(Name, This.Device.Printer.Paper_Length);
                case DevModeField.Printer_Paper_Size:
                    return DevModeRecord.Create(Name, This.Device.Printer.Paper_Size);
                case DevModeField.Printer_Paper_Width:
                    return DevModeRecord.Create(Name, This.Device.Printer.Paper_Width);
                case DevModeField.Printer_PrintQuality_X:
                    return DevModeRecord.Create(Name, This.Device.Printer.PrintQuality_X);
                case DevModeField.Printer_PrintQuality_Y:
                    return DevModeRecord.Create(Name, This.Printer_PrintQuality_Y);
                case DevModeField.Printer_Scale:
                    return DevModeRecord.Create(Name, This.Device.Printer.Scale);
                case DevModeField.Printer_TrueTypeFontOptions:
                    return DevModeRecord.Create(Name, This.Printer_TrueTypeFontOptions);
                default:
                    return default;

            }
        }

        public static List<DevModeRecord> AllRecords(this DevModeA This)
        {
           /* var _ret = (
                from x in Enum.GetValues(typeof(DevModeField)).OfType<DevModeField>()
                where This.Fields.HasFlag(x)
                let v = GetField(This, x)
                where v is { }
                select v
                ).ToList();*/
            var ret = Enum.GetValues(typeof(DevModeField)).OfType<DevModeField>();
            var records = ret
                .Where(x => This.Fields.HasFlag(x))
                .Where(v => GetField(This, v) != null)
                .Select(v => GetField(This, v)).ToList();
            /*var qry = from cust in db.Customers
                      where cust.IsActive
                      select cust;
            var qry = db.Customers.Where(cust => cust.IsActive);*/

            return records;
        }

        public static IDictionary<DevModeField, DevModeRecord> AllRecordsDictionary(this DevModeA This)
        {
            var ret = This.AllRecords().ToDictionary(x => x.Name, x => x);
            return ret;
        }
    }

}
