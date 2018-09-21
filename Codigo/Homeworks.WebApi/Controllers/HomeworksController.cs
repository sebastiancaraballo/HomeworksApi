using System;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess;
using Homeworks.WebApi.Models;
using Homeworks.WebApi.Filters;

namespace Homeworks.WebApi.Controllers
{
    [ProtectFilter("Admin")]
    [Route("api/[controller]")]
    public class HomeworksController : Controller
    {
        private IHomeworkLogic homeworks;

        public HomeworksController(IHomeworkLogic homeworks = null) : base()
        {
            if (homeworks == null)
            {
                var context = ContextFactory.GetNewContext();
                var homeworkRepo = new HomeworkRepository(context);
                var exerciseRepo = new ExerciseRepository(context);
                homeworks = new HomeworkLogic(homeworkRepo, exerciseRepo);
            }
            this.homeworks = homeworks;
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

        protected override void Dispose(bool disposing) 
        {
            try {
                base.Dispose(disposing);
            } finally {
                homeworks.Dispose();
            }
        }
    }
}
