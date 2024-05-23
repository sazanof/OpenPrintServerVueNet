using OpenPrintServerVueNet.Server.Classes.DTO;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Responses
{
    public class AppInstalledDTO : AppNotInstalledDTO
    {
        public UserDTO? User { get; set; }

        public bool? Authenticated { get; set; }
    }
}
