using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Classes;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Models;
using System.Data.Common;
using System.Management;
using System.Net;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/printers")]
    public class PrinterController : Controller
    {
        private readonly ApplicationContext _db;

        public PrinterController(ApplicationContext db)
        {
            _db = db;
        }

        public Snmp? snmp = null;

        [HttpGet("sync/{id?}")]
        public async Task<IActionResult> SyncPrinters(int id = 0)
        {
            return await Task.Run(() =>
            {
                var query = $"SELECT *  FROM Win32_Printer";
                if(id > 0)
                {
                    var founded = _db.Printers.Find(id);
                    if(founded != null)
                    {
                        query += $" Where Name = '{founded.Name}'";
                    }
                }
                //return Ok(query);
                var searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject entry in searcher.Get())
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
                            var existingPort = _db.PrinterPorts.FirstOrDefault(p => p.Name == creatingPort.Name);
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
                            _db.PrinterPorts.AddRange(ports);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Printer PRNT;

                    var current = _db.Printers.FirstOrDefault(p => p.DeviceID == printer.DeviceID);

                    if (current == null)
                    {
                        _db.Printers.Add(printer);
                        PRNT = printer;
                    }
                    else
                    {
                        current.PortName = printer.PortName;
                        current.PrinterStatus = printer.PrinterStatus;
                        current.Shared = printer.Shared;
                        current.ShareName = printer.ShareName;
                        current.SystemName = printer.SystemName;
                        current.Location = printer.Location;
                        current.Description = printer.Description;
                        current.Comment = printer.Comment;
                        current.Caption = printer.Caption;
                        current.Default = printer.Default;
                        current.Network = printer.Network;
                        current.Name = printer.Name;

                        PRNT = current;
                    }

                    var targetPort = _db.PrinterPorts.FirstOrDefault(_port => _port.Name == PRNT.PortName);

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

                            if (SnmpName != null) PRNT.SnmpName = SnmpName;
                            if (SnmpSerialNumber != null) PRNT.SnmpSerialNumber = SnmpSerialNumber;
                            if (SnmpSystemName != null) PRNT.SnmpSystemName = SnmpSystemName;
                            if (SnmpLocation != null) PRNT.SnmpLocation = SnmpLocation;
                            if (SnmpUptime != null) PRNT.SnmpUptime = SnmpUptime;
                            if (SnmpCountTotal != null) PRNT.SnmpCountTotal = UInt64.Parse(SnmpCountTotal);
                            if (SnmpCountUptime != null) PRNT.SnmpCountUptime = UInt64.Parse(SnmpCountUptime);

                            var consumtables = PrepareConsumables(snmp, PRNT);
                            foreach (var consumtable in consumtables)
                            {
                                var existing = _db.Consumables.FirstOrDefault(
                                    cons => cons.Printer == consumtable.Printer && cons.Name == consumtable.Name
                                    );
                                if (existing == null)
                                {
                                    PRNT.Consumables.Add(consumtable);
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

                }
                _db.SaveChanges();

                return Ok(_db.Printers);
            });


        }

        [HttpGet("")]
        public async Task<IActionResult> GetPrintersList()
        {
            return await Task.Run(() =>
            {
                return Ok(_db.Printers.Include(x => x.Ports).Include(c => c.Consumables));
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrinter(int id)
        {
            return await Task.Run(() =>
            {
                return Ok(
                    _db.Printers
                    .Include(x => x.Ports)
                    .Include(c => c.Consumables)
                    .FirstOrDefault(p => p.Id == id)
                    );
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePrinter(int id)
        {
            var printer = _db.Printers.FirstOrDefault(p => p.Id == id);
            _db.Remove<Printer>(printer);
            _db.SaveChanges();
            return Ok();
        }

        protected List<Consumables> PrepareConsumables(Snmp snmp, Printer printer)
        {
            var consumable = new List<Consumables>();

            var consumableNames = snmp.Walk(SnmpOIDs.ConsumablesName);
            var consumableTypes = snmp.Walk(SnmpOIDs.ConsumablesType);
            var consumableRemains = snmp.Walk(SnmpOIDs.ConsumablesRemains);
            var consumableTotal = snmp.Walk(SnmpOIDs.ConsumablesCapacity);
            var consumableColor = snmp.Walk(SnmpOIDs.Color);

            var i = 0;

            foreach (var name in consumableNames)
            {
                consumable.Add(new Consumables()
                {
                    Name = name.Data.ToString(),
                    Type = int.Parse(consumableTypes.ElementAt(i).Data.ToString()),
                    Capacity = int.Parse(consumableTotal.ElementAt(i).Data.ToString()),
                    Remains = int.Parse(consumableRemains.ElementAt(i).Data.ToString()),
                    Printer = printer
                });
                i++;
            }

            var catriges = consumable.Where(item =>
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
                foreach (var cons in consumable)
                {
                    if (cons.Name == cartrige.Name)
                    {
                        consumable.ElementAt(cidx).Color = cons.Color;
                        break;
                    }
                    cidx++;
                }

                j++;
            }
            return consumable;
        }
    }
}
