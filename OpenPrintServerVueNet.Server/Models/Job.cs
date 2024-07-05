using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPrintServerVueNet.Models
{
    [Table("Jobs")]
    [Index(nameof(JobId))]
    [Index(nameof(Username))]
    [Index(nameof(Status))]
    [Index(nameof(Submitted))]
    public class Job
    {
        [Key]
        public Int64 Id { get; set; }

        public Int64 JobId { get; set; }

        [MaxLength(100)]
        public string? PrinterName { get; set; }

        [MaxLength(100)]
        public string? MachineName { get; set; } = null;

        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(100)]
        public string NotifyName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string DriverName { get; set; } = string.Empty;

        public int? Printer_Orientation { get; set; }

        public int? Printer_Paper_Size { get; set;}

        public short? Printer_Copies { get; set; }

        public int? Printer_DefaultSource { get; set; }

        public int? Printer_PrintQuality_X { get; set; }

        public int? Printer_PrintQuality_Y { get; set; }

        public int? Printer_Palette { get; set; }

        public string? Printer_FormName { get; set; }

        public int? Status { get; set; }

        [MaxLength(100)]
        public string? StatusString { get; set;} = null;

        public string? Document { get; set; } = null;

        public int? Priority { get; set; }

        public int? Position { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? Submitted { get; set; }

        public string? Time { get; set; }

        public int? PagesTotal { get; set; }

        public int? PagesPrinted { get; set; }

        public int? BytesTotal { get; set; }

        public int? BytesPrinted { get; set; }

        public bool? Synced { get; set; }

        public int? PrinterId { get; set; }

        public Printer? Printer { get; set; }
    }
}
