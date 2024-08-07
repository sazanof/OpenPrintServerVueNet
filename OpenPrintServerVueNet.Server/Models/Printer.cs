﻿using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OpenPrintServerVueNet.Server.Models
{
    [Index(nameof(DeviceID))]
    public class Printer
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

        public ICollection<Job> Jobs { get; set; }

        public ICollection<Consumables> Consumables { get; set; }

        public ICollection<PrinterPort> Ports { get; set; }

        public Printer()
        {
            Ports = new List<PrinterPort>();
            Consumables = new List<Consumables>();
            Jobs = new List<Job>(); 
        }

       
    }
}
