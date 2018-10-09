using System;
using Microsoft.AspNetCore.Mvc;
using Homeworks.WebApi.Models;
using Homeworks.BusinessLogic.Interface;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserLogic users;

        public UsersController(IUserLogic users) : base()
        {
            this.users = users;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(UserModel.ToModel(users.GetAll()));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = users.Get(id);
            if (user == null) {
                return NotFound();
            }
            return Ok(UserModel.ToModel(user));
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserModel model)
        {
            try {
                var user = users.Create(UserModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = user.Id }, UserModel.ToModel(user));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing) 
        {
            try {
                base.Dispose(disposing);
            } finally {
                users.Dispose();
            }
        }
    }
}
