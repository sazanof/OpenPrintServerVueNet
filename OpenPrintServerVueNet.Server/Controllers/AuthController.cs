using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Classes.DTO;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Models;
using OpenPrintServerVueNet.Server.Payload;
using OpenPrintServerVueNet.Server.Responses;
using System.Security.Claims;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _db;

        public AuthController(
            ApplicationContext ctx,
            SignInManager<User> manager,
            UserManager<User> userManager
            )
        {
            _db = ctx;
            _signInManager = manager;
            _userManager = userManager;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckAuthState()
        {
            var state = User.Identity?.IsAuthenticated;
            //https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            var currentUser = await _userManager.GetUserAsync(User);
            var responce = new AuthResponceDTO()
            {
                Authenticated = (bool)(state != null ? state : false),
                User = UserDTO.FromUser(currentUser)
            };
            return Ok(responce);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult LoginGet()
        {
            return Redirect("/");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginPayload loginPayload)
        {
            var user = _db.Users.FirstOrDefault(u=>u.UserName == loginPayload.Username);
            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginPayload.Password, false, false); // TODO remember me
                if (result.Succeeded)
                {
                    var res = new AuthResponceDTO() { 
                        Authenticated = true, 
                        User = UserDTO.FromUser(user) 
                    };
                    return Ok(res);
                }
                throw new Exception("Username or password incorrect!");
            }
            throw new Exception("User not found");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return Ok(new AuthResponceDTO() { 
                Authenticated = false,
                User = null
            });
        }
    }
}
