
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Enums;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Payload;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {

        ApplicationContext db;
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public UserController(
            ApplicationContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
            )
        {
            db = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            
            return Ok(db.Users.FirstOrDefault(u=>u.UserName == username));
           // return Ok(db.Users.FirstOrDefault(user => user.UserName == username));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(db.Users);
            // return Ok(db.Users.FirstOrDefault(user => user.UserName == username));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserPayload userPayload)
        {
            var founded = db.Users.FirstOrDefault(u => u.UserName == userPayload.UserName || u.Email == userPayload.Email);
            if (founded == null) {
                var hasher = new PasswordHasher<User>();
                var user = new User()
                {
                    UserName = userPayload.UserName,
                    PasswordHash = hasher.HashPassword(null, userPayload.Password),
                    Email = userPayload.Email,
                    Firstname = userPayload.FirstName,
                    Lastname = userPayload.LastName
                };

                db.Users.Add(user);

                await db.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, RoleEnum.Admin);
                await _userManager.AddToRoleAsync(user, RoleEnum.Statist);
                await _userManager.AddToRoleAsync(user, RoleEnum.Viewer);

                return Ok(user);
            } else
            {
                return BadRequest();
            }
            
        }
    }
}
