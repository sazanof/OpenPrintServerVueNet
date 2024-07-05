using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Classes.Spool;
using OpenPrintServerVueNet.Server.Classes;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Models;
using System.Management;
using System.Net;

namespace OpenPrintServerVueNet.Server.Helpers
{
    public class PrinterHelper
    {
        public static Printer? AddPrinter(PrintDeviceData data, ApplicationContext db, bool save = true)
        {

            var query = $"SELECT *  FROM Win32_Printer  Where Name = '{data.PrintDevice_PrinterName().Value}'";
            var entry = new ManagementObjectSearcher(query).Get().Cast<ManagementObject>().FirstOrDefault();

            if (entry != null)
            {
                var printer = new Printer()
                {
                    Attributes = (uint?)entry.GetPropertyValue("Attributes"),
                    Name = (string?)entry.GetPropertyValue("Name"),
                    Default = (bool?)entry.GetPropertyValue("Default"),
                    Direct = (bool?)entry.GetPropertyValue("Direct"),
                    DriverName = (string?)entry.GetPropertyValue("DriverName"),
                    DeviceID = (string?)entry.GetPropertyValue("DeviceID"),
                    Caption = (string?)entry.GetPropertyValue("Caption"),
                    Description = (string?)entry.GetPropertyValue("Description"),
                    Comment = (string?)entry.GetPropertyValue("Comment"),
                    Capabilities = (ushort[]?)entry.GetPropertyValue("Capabilities"),
                    PaperSizesSupported = (ushort[]?)entry.GetPropertyValue("PaperSizesSupported"),
                    PrinterPaperNames = (string[]?)entry.GetPropertyValue("PrinterPaperNames"),
                    ExtendedPrinterStatus = (ushort?)entry.GetPropertyValue("ExtendedPrinterStatus"),
                    Hidden = (bool?)entry.GetPropertyValue("Hidden"),
                    HorizontalResolution = (UInt32?)entry.GetPropertyValue("HorizontalResolution"),
                    InstallDate = (DateTime?)entry.GetPropertyValue("InstallDate"),
                    Local = (bool?)entry.GetPropertyValue("Local"),
                    Network = (bool?)entry.GetPropertyValue("Network"),
                    Location = (string?)entry.GetPropertyValue("Location"),
                    PortName = (string?)entry.GetPropertyValue("PortName"),
                    PrinterStatus = (ushort?)entry.GetPropertyValue("PrinterStatus"),
                    PrintJobDataType = (string?)entry.GetPropertyValue("PrintJobDataType"),
                    PrintProcessor = (string?)entry.GetPropertyValue("PrintProcessor"),
                    Shared = (bool?)entry.GetPropertyValue("Shared"),
                    ShareName = (string?)entry.GetPropertyValue("ShareName"),
                    SystemName = (string?)entry.GetPropertyValue("SystemName"),
                    VerticalResolution = (UInt32?)entry.GetPropertyValue("VerticalResolution")
                };


                var existing = db.Printers.FirstOrDefault(p => p.Name == printer.Name);

                if (existing == null)
                {
                    db.Printers.Add(printer);

                    if (save)
                    {
                        db.SaveChanges();
                    }

                    return db.Printers
                        .Include(p => p.Ports)
                        .Include(p => p.Consumables)
                        .FirstOrDefault(p=>p.Id == printer.Id);

                }
            }

            

            return null;
        }

        /// <summary>
        /// Remove printer
        /// </summary>
        /// <param name="data"></param>
        /// <param name="db"></param>
        public static Printer? DeletePrinter(PrintDeviceData data, ApplicationContext db, bool save = true)
        {
            var name = data.PrintDevice_PrinterName().Value;
            Console.WriteLine("\r\nDelete printer {0}\r\n", name);
            var entry = db.Printers.FirstOrDefault(p => p.Name == name);
            if (entry != null)
            {
                db.Printers.Remove(entry);
                if (save)
                {
                    db.SaveChanges();
                }
                return entry;
            }
            return null;

        }

        public static ICollection<PrinterPort>? AddOrEditPrinterPort(Printer printer, ApplicationContext db, bool save = true)
        {
            var ports = new List<PrinterPort>();
            try
            {
                var _searcher = new ManagementObjectSearcher($"SELECT *  FROM Win32_TCPIPPrinterPort WHERE Name = '" + printer.PortName + "'");
                foreach (var port in _searcher.Get())
                {
                    var creatingPort = new PrinterPort()
                    {
                        Name = (string?)port.GetPropertyValue("Name"),
                        Description = (string?)port.GetPropertyValue("Description"),
                        HostAddress = (string?)port.GetPropertyValue("HostAddress"),
                        Printer = printer
                    };
                    var existingPort = db.PrinterPorts.FirstOrDefault(p => p.Name == creatingPort.Name);
                    if (existingPort == null)
                    {
                        ports.Add(creatingPort);
                    }
                    else
                    {
                        existingPort.HostAddress = creatingPort.HostAddress;
                        existingPort.Description = creatingPort.Description;
                    }
                    Console.WriteLine("\r\n\r\n {0} \r\n\r\n", port.GetPropertyValue("Name"));
                }
                if (ports.Count > 0)
                {
                    db.PrinterPorts.AddRange(ports);
                }
                if (save)
                {
                    db.SaveChanges();
                }


                return printer.Ports;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

            return null;
        }

        public static Printer SyncSnmpValues(Printer printer, ApplicationContext db)
        {
            var targetPort = db.PrinterPorts.FirstOrDefault(_port => _port.Name == printer.PortName);

            if (targetPort != null)
            {
                try
                {
                    Console.WriteLine($"\r\nFound port {targetPort.Name}\r\n");

                    var ip = IPAddress.Parse(targetPort.HostAddress);
                    var snmp = new Snmp(ip, Lextm.SharpSnmpLib.VersionCode.V1);

                    targetPort.MacAddress = Arp.LocateMacAddress(ip).ToString();

                    var SnmpName = snmp.Get(SnmpOIDs.Name)?.First().Data.ToString();
                    var SnmpSerialNumber = snmp.Get(SnmpOIDs.SerialNumber)?.First().Data.ToString();
                    var SnmpSystemName = snmp.Get(SnmpOIDs.SystemName)?.First().Data.ToString();
                    var SnmpFQDN = snmp.Get(SnmpOIDs.FQDN)?.First().Data.ToString();
                    var SnmpLocation = snmp.Get(SnmpOIDs.Location)?.First().Data.ToString();
                    var SnmpUptime = snmp.Get(SnmpOIDs.Uptime)?.First().Data.ToString();
                    var SnmpManufacturerOID = snmp.Get(SnmpOIDs.VendorOID)?.First().Data.ToString();
                    var SnmpCountTotal = snmp.Get(SnmpOIDs.CountTotal)?.First().Data.ToString();
                    var SnmpCountUptime = snmp.Get(SnmpOIDs.CountUptime)?.First().Data.ToString();

                    if (SnmpName != null) printer.SnmpName = SnmpName;
                    if (SnmpSerialNumber != null) printer.SnmpSerialNumber = SnmpSerialNumber;
                    if (SnmpSystemName != null) printer.SnmpSystemName = SnmpSystemName;
                    if (SnmpLocation != null) printer.SnmpLocation = SnmpLocation;
                    if (SnmpUptime != null) printer.SnmpUptime = SnmpUptime;
                    if (SnmpCountTotal != null) printer.SnmpCountTotal = UInt64.Parse(SnmpCountTotal);
                    if (SnmpCountUptime != null) printer.SnmpCountUptime = UInt64.Parse(SnmpCountUptime);

                    var consumtables = PrepareConsumables(snmp, printer);
                    foreach (var consumtable in consumtables)
                    {
                        var existing = db.Consumables.FirstOrDefault(
                            cons => cons.Printer == consumtable.Printer && cons.Name == consumtable.Name
                            );
                        if (existing == null)
                        {
                            printer.Consumables.Add(consumtable);
                            Console.WriteLine($"\r\nAdd consumable:{consumtable.Name}\r\n");
                        }
                        else
                        {
                            existing.Capacity = consumtable.Capacity;
                            existing.Remains = consumtable.Remains;
                            existing.Color = consumtable.Color;
                            existing.Type = consumtable.Type;
                            Console.WriteLine($"\r\nEdit consumable:{existing.Name}\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return printer;
        }

        protected static List<Consumables> PrepareConsumables(Snmp snmp, Printer printer)
        {
            var consumables = new List<Consumables>();

            var consumableNames = snmp.Walk(SnmpOIDs.ConsumablesName);
            var consumableTypes = snmp.Walk(SnmpOIDs.ConsumablesType);
            var consumableRemains = snmp.Walk(SnmpOIDs.ConsumablesRemains);
            var consumableTotal = snmp.Walk(SnmpOIDs.ConsumablesCapacity);
            var consumableColor = snmp.Walk(SnmpOIDs.Color);

            var i = 0;

            foreach (var name in consumableNames)
            {
                consumables.Add(new Consumables()
                {
                    Name = name.Data.ToString(),
                    Type = int.Parse(consumableTypes.ElementAt(i).Data.ToString()),
                    Capacity = int.Parse(consumableTotal.ElementAt(i).Data.ToString()),
                    Remains = int.Parse(consumableRemains.ElementAt(i).Data.ToString()),
                    Printer = printer
                });
                i++;
            }

            var catriges = consumables.Where(item =>
            {
                int type = item.Type;
                var allowedTypes = new int[] { (int)ConsumablesType.Toner, (int)ConsumablesType.Ink };
                return allowedTypes.Contains(type);
            });
            var j = 0;
            foreach (var cartrige in catriges)
            {
                cartrige.Color = consumableColor.ElementAt(j).Data.ToString();
                var cidx = 0;
                foreach (var cons in consumables)
                {
                    if (cons.Name == cartrige.Name)
                    {
                        consumables.ElementAt(cidx).Color = cons.Color;
                        break;
                    }
                    cidx++;
                }

                j++;
            }
            return consumables;
        }
    }
}
