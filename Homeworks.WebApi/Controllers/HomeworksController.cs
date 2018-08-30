using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homeworks.BusinessLogic;
using Homeworks.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeworksController : Controller
    {
        private HomeworkLogic homeworks;

        public HomeworksController() 
        {
            homeworks = new HomeworkLogic();
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(homeworks.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var homework = homeworks.Get(id);
            if (homework == null) {
                return NotFound();
            }
            return Ok(new HomeworkModel(homework));
        }

        [HttpGet("{id}/Exercises")]
        public IActionResult GetExercise(Guid id)
        {
            var homework = homeworks.Get(id);
            if (homework == null) {
                return NotFound();
            }
            return Ok(homework.Exercises);
        }

        [HttpPost]
        public IActionResult Post([FromBody]HomeworkModel homeworkModel)
        {
            var homework = homeworks.Create(homeworkModel.ToEntity());
            return Ok(homework);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]Homework homework)
        {
            homework = homeworks.Update(id, homework);
            return Ok(homework);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            homeworks.Remove(id);
            return NoContent();
        }
    }
}
