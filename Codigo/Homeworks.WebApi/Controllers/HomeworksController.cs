using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic;
using Homeworks.WebApi.Models;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class HomeworksController : Controller
    {
        private HomeworkLogic homeworks;

        public HomeworksController() : base() {
            homeworks = new HomeworkLogic();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(HomeworkModel.ToModel(homeworks.GetAll()));
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            var homework = homeworks.Get(id);
            if (homework == null) {
                return NotFound();
            }
            return Ok(HomeworkModel.ToModel(homework));
        }

        [HttpPost("{id}/Exercises", Name = "AddExercise")]
        public IActionResult PostExercise(Guid id, [FromBody]ExerciseModel exercise)
        {
            var newExercise = homeworks.AddExercise(id, ExerciseModel.ToEntity(exercise));
            if (newExercise == null) {
                return BadRequest();
            }
            return CreatedAtRoute("GetExercise", new { id = newExercise.Id }, ExerciseModel.ToModel(newExercise));
        }

        [HttpPost]
        public IActionResult Post([FromBody]HomeworkModel model)
        {
            try {
                var homework = homeworks.Create(HomeworkModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = homework.Id }, HomeworkModel.ToModel(homework));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]HomeworkModel model)
        {
            try {
                var homework = homeworks.Update(id, HomeworkModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = homework.Id }, HomeworkModel.ToModel(homework));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            homeworks.Remove(id);
            return NoContent();
        }

        protected override void Dispose(bool disposing) {
            try {
                base.Dispose(disposing);
            } finally {
                homeworks.Dispose();
            }
        }
    }
}
