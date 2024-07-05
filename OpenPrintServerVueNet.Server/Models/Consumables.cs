using System.ComponentModel.DataAnnotations;

namespace OpenPrintServerVueNet.Server.Models
{
    public class Consumables
    {
        [Key]
        public UInt64 Id { get; set; }

        public int Type { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public int Remains { get; set; }

        public int Capacity { get; set; }

        public int? PrinterId { get; set; }

        public Printer? Printer { get; set; }
    }
}
