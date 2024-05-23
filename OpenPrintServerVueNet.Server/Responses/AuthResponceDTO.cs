using OpenPrintServerVueNet.Server.Classes.DTO;

namespace OpenPrintServerVueNet.Server.Responses
{
    public class AuthResponceDTO
    {
        public bool Authenticated { get; set; }
        
        public UserDTO? User { get; set; }    
    }
}
