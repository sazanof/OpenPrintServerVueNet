using Microsoft.AspNetCore.Mvc;
using OpenPrintServerVueNet.Server.Contexts;
using OpenPrintServerVueNet.Server.Models;

namespace OpenPrintServerVueNet.Server.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {

        ApplicationContext db;

        public UserController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet("{username}")]
        public IActionResult GetUserByUsername(string username)
        {
            db.Users.Add(new User() { UserName = "admin" });
            db.SaveChanges();
            return Ok();
           // return Ok(db.Users.FirstOrDefault(user => user.UserName == username));
        }
    }
}
