using Microsoft.AspNetCore.Mvc;
using Homeworks.WebApi.Models;
using Homeworks.WebApi.Filters;
using Homeworks.BusinessLogic.Interface;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private ISessionLogic sessions;

        public TokenController(ISessionLogic sessions) : base()
        {
            this.sessions = sessions;
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginModel model) {
            var token = sessions.CreateToken(model.UserName, model.Password);
            if (token == null) 
            {
                return BadRequest("Invalid user/password");
            }
            return Ok(token);
        }

        [ProtectFilter("Admin")]
        [HttpGet("Check")]
        public IActionResult CheckLogin() {
            return Ok(new UserModel(sessions.GetUser(Request.Headers["Authorization"])));
        }

        protected override void Dispose(bool disposing) 
        {
            try {
                base.Dispose(disposing);
            } finally {
                sessions.Dispose();
            }
        }
    }
}
