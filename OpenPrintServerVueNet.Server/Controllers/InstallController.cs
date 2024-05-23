using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Classes.DTO;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Payload;
using OpenPrintServerVueNet.Server.Responses;
using System.Text;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [ApiController]
    [Route("/api/install")]
    public class InstallController : Controller
    {
        ApplicationContext db;

        public InstallController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        public IActionResult InstallApp(InstallPayload data)
        {
            // Create a user
            var hasher = new PasswordHasher<User>();
            var user = new User()
            {
                UserName = data.Username,
                PasswordHash = hasher.HashPassword(null, data.Password),
                Email = data.Email,
                Firstname = data.Firstname,
                Lastname = data.Lastname
            };
            var current = db.Users.FirstOrDefault(u => u.UserName == data.Username);
            if (current == null)
            {
                db.Users.Add(user);

                var dto = new UserDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email
                };

                // Clear Config
                db.Config.RemoveRange(db.Config.ToList()); // ? or not delete
                                                           // Add Config
                var conf = new List<Config>()
                {
                    new Config() { Key = ConfigEnum.IsInstalled, Value = "true"},
                    new Config() { Key = ConfigEnum.InstallDate, Value = DateTime.Now.ToString()},
                };

                var missing = conf.Where(x => !db.Config.Any(z => z.Key == x.Key)).ToList();
                db.Config.AddRange(missing);

                db.SaveChanges();

                var res = new AppInstalledDTO()
                {
                    Authenticated = false,
                    User = dto,
                    IsInstalled = true,
                };

                return Ok(res);
            }

            return BadRequest();

        }
    }
}
