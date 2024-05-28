using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OpenPrintServerVueNet.Server.Models
{
    [Index(nameof(Section))]
    public class SnmpVariable
    {
        [Key]
        public UInt64 Id { get; set; }

        [Required]
        public string? Section { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Method { get; set; } // walk, get

        public ICollection<SnmpValue> Values { get; set; }

        public SnmpVariable()
        {
            Values = new List<SnmpValue>();
        }

    }
}
