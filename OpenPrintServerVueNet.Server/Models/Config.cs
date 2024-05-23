using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenPrintServerVueNet.Server.Models
{
    [Table("Config")]
    [Index(nameof(Key),IsUnique = true)]
    public class Config
    {
        public int Id { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }

        public DateTime? Created { get; set; }
    }
}
