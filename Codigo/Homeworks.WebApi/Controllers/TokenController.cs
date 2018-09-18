using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic;
using Homeworks.WebApi.Models;
using Homeworks.WebApi.Filters;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private SessionLogic sessions;

        public TokenController() 
        {
            sessions = new SessionLogic();
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
