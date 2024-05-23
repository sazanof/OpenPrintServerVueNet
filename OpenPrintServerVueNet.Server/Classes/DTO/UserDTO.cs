using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Classes.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public static UserDTO? FromUser(User? user)
        {
            return user == null ? null : new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email
            };
        }
    }
}
