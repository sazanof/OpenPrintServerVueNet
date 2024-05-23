using Microsoft.AspNetCore.Identity;
namespace OpenPrintServerVueNet.Server.Models
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
    }
}
