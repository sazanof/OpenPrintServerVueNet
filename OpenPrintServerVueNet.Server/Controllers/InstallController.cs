using Humanizer;
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
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public InstallController(
            ApplicationContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
            )
        {
            db = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> InstallApp(InstallPayload data)
        {
            User _user;

            createRoles();

            // Create a user
            var current = db.Users.FirstOrDefault(u => u.UserName == data.Username);

            var hasher = new PasswordHasher<User>();

            var user = new User()
            {
                UserName = data.Username,
                PasswordHash = hasher.HashPassword(null, data.Password),
                Email = data.Email,
                Firstname = data.Firstname,
                Lastname = data.Lastname
            };

            
            if (current == null)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, RoleEnum.Admin);
                await _userManager.AddToRoleAsync(user, RoleEnum.Statist);
                await _userManager.AddToRoleAsync(user, RoleEnum.Viewer);

                _user = user;
            }
            else
            {
                current.Firstname = user.Firstname;
                current.Lastname = user.Lastname;
                current.PasswordHash = user.PasswordHash;
                current.Email = user.Email;
                await db.SaveChangesAsync();

                await _userManager.AddToRoleAsync(current, RoleEnum.Admin);
                await _userManager.AddToRoleAsync(current, RoleEnum.Statist);
                await _userManager.AddToRoleAsync(current, RoleEnum.Viewer);

                _user = current;
            }

           

            // Clear Config
            db.Config.RemoveRange(db.Config.ToList()); // ? or not delete
                                                       // Add Config
            var conf = new List<Config>()
                {
                    new Config() { Key = ConfigEnum.IsInstalled, Value = "true"},
                    new Config() { Key = ConfigEnum.InstallDate, Value = DateTime.Now.ToString()},
                };

            //var missing = conf.Where(x => !db.Config.Any(z => z.Key == x.Key)).ToList();

            db.Config.AddRange(conf);

            await db.SaveChangesAsync();

            var dto = new UserDTO()
            {
                Id = _user.Id,
                UserName = _user.UserName,
                FirstName = _user.Firstname,
                LastName = _user.Lastname,
                Email = _user.Email
            };

            var res = new AppInstalledDTO()
            {
                Authenticated = false,
                User = dto,
                IsInstalled = true,
            };

            return Ok(res);

        }

        private async void createRoles()
        {
            var adminRole = await _roleManager.FindByNameAsync(RoleEnum.Admin);

            if (adminRole == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleEnum.Admin));
            }

            var statistRole = await _roleManager.FindByNameAsync(RoleEnum.Statist);

            if (statistRole == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleEnum.Statist));
            }

            var viewerRole = await _roleManager.FindByNameAsync(RoleEnum.Viewer);

            if (viewerRole == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleEnum.Viewer));
            }
        }
    }
}
