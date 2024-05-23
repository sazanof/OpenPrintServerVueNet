using OpenPrintServerVueNet.Classes.Spool.Native.DevMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPrintServerVueNet.Classes.DTO
{
    public class DevModeDTO
    {
        public PrinterOrientation? Printer_Orientation { get; set; }

        public PrinterPaperSize? Printer_Paper_Size { get; set; }

        public short? Printer_Copies { get; set; }

        public int? Printer_DefaultSource { get; set; }

        public int? Printer_PrintQuality_X { get; set; }

        public int? Printer_PrintQuality_Y { get; set; }

        public PrinterPalette? Printer_Palette { get; set; }

        public string? Printer_FormName { get; set; }
    }
}
