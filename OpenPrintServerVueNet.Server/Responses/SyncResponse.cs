using Microsoft.EntityFrameworkCore;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Responses
{
    public class SyncResponse
    {
        public SyncStatusEnum? Status { get; set; }

        public DbSet<Printer>? Printers { get; set; }
    }
}
