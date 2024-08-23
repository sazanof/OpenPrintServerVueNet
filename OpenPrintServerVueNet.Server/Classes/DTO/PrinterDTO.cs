using OpenPrintServerVueNet.Classes.DTO;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Classes.DTO
{
    public class PrinterDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool? Default { get; set; }

        public bool? Direct { get; set; }

        public string? DriverName { get; set; }

        public string? DeviceID { get; set; }

        public string? Caption { get; set; }

        public string? Description { get; set; }

        public string? Comment { get; set; }

        public UInt32? Attributes { get; set; }

        public UInt16[]? Capabilities { get; set; }

        public UInt16[]? PaperSizesSupported { get; set; }

        public string[]? PrinterPaperNames { get; set; }

        public UInt16? ExtendedPrinterStatus { get; set; }

        public bool? Hidden { get; set; }

        public UInt32? HorizontalResolution { get; set; }

        public DateTime? InstallDate { get; set; }

        public bool? Local { get; set; }

        public bool? Network { get; set; }

        public string? Location { get; set; }

        public string? PortName { get; set; }

        public UInt16? PrinterStatus { get; set; }

        public string? PrintJobDataType { get; set; }

        public string? PrintProcessor { get; set; }

        public bool? Shared { get; set; }

        public string? ShareName { get; set; }

        public string? SystemName { get; set; }

        public UInt32? VerticalResolution { get; set; }

        public string? SnmpName { get; set; }

        public string? SnmpManufacturerOID { get; set; }

        public string? SnmpUptime { get; set; }

        public string? SnmpContact { get; set; }

        public string? SnmpFQDN { get; set; }

        public string? SnmpLocation { get; set; }

        public string? SnmpSystemName { get; set; }

        public string? SnmpSerialNumber { get; set; }

        public UInt64? SnmpCountTotal { get; set; }

        public UInt64? SnmpCountUptime { get; set; }

        public string? OperatorMessage { get; set; }

        public IEnumerable<ConsumableDTO>? Consumables { get; set; }

        public IEnumerable<PrinterPortDTO>? Ports { get; set; }

        public PrinterDTO(Printer? printer)
        {
            Id = printer.Id;

            Name = printer.Name;

            DriverName = printer.DriverName;

            DeviceID = printer.DeviceID;

            Caption = printer.Caption;

            Description = printer.Description;

            Comment = printer.Comment;

            Attributes = printer.Attributes;

            Capabilities = printer.Capabilities;

            PaperSizesSupported = printer.PaperSizesSupported;

            PrinterPaperNames = printer.PrinterPaperNames;

            ExtendedPrinterStatus = printer.ExtendedPrinterStatus;

            Hidden = printer.Hidden;

            HorizontalResolution = printer.HorizontalResolution;

            InstallDate = printer.InstallDate;

            Local = printer.Local;

            Network = printer.Network;

            Location = printer.Location;

            PortName = printer.PortName;

            PrinterStatus = printer.PrinterStatus;

            PrintJobDataType = printer.PrintJobDataType;

            PrintProcessor = printer.PrintProcessor;

            Shared = printer.Shared;

            ShareName = printer.ShareName;

            SystemName = printer.SystemName;

            VerticalResolution = printer.VerticalResolution;

            SnmpName = printer.SnmpName;

            SnmpManufacturerOID = printer.SnmpManufacturerOID;

            SnmpUptime = printer.SnmpUptime;

            SnmpContact = printer.SnmpContact;

            SnmpFQDN = printer.SnmpFQDN;

            SnmpLocation = printer.SnmpLocation;

            SnmpSystemName = printer.SnmpSystemName;

            SnmpSerialNumber = printer.SnmpSerialNumber;

            SnmpCountTotal = printer.SnmpCountTotal;

            SnmpCountUptime = printer.SnmpCountUptime;

            OperatorMessage = printer.OperatorMessage;

            Consumables = printer.Consumables.Select(c => new ConsumableDTO
            {
                Id = c.Id,
                Type = c.Type,
                Capacity = c.Capacity,
                Color = c.Color,
                Name = c.Name,
                Remains = c.Remains,
                PrinterId = c.PrinterId
            });

            Ports = printer.Ports.Select(p => new PrinterPortDTO { 
                Id = p.Id,
                Name = p.Name,
                HostAddress = p.HostAddress,
                MacAddress = p.MacAddress,
                Description = p.Description,
                PrinterId = p.PrinterId
            });
        }
    }
}
