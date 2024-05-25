using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Models;
using System.Data.Common;
using System.Management;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/printers/sync")]
    public class PrinterController : Controller
    {
        private readonly ApplicationContext _db;

        public PrinterController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> SyncPrinters()
        {
            return await Task.Run(() =>
            {
                var searcher = new ManagementObjectSearcher("SELECT *  FROM Win32_Printer");
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


                    var current = _db.Printers.FirstOrDefault(p => p.DeviceID == printer.DeviceID);
                    if (current == null)
                    {
                        _db.Printers.Add(printer);
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
                    }
                }
                _db.SaveChanges();
                return Ok(_db.Printers);
            });


        }

        [HttpGet("/api/printers")]
        public async Task<IActionResult> GetPrintersList()
        {
            return await Task.Run(() =>
            {
                return Ok(_db.Printers.Include(x => x.Ports));
            });
        }
    }
}
