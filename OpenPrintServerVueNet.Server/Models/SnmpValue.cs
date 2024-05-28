using System.ComponentModel.DataAnnotations;

namespace OpenPrintServerVueNet.Server.Models
{
    public class SnmpValue
    {
        [Key]
        public UInt64 Id;

        [Required]
        public string? Value {  get; set; }  

        public Printer? Printer { get; set; }

        public SnmpVariable? Variable { get; set; }
    }
}
