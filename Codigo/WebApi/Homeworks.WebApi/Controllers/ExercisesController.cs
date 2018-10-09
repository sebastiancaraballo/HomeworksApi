using System;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic.Interface;
using Homeworks.WebApi.Models;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ExercisesController : Controller
    {
        private IExerciseLogic exercises;

        public ExercisesController(IExerciseLogic exercises) : base()
        {
            this.exercises = exercises;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ExerciseModel.ToModel(exercises.GetAll()));
        }

        [HttpGet("{id}", Name = "GetExercise")]
        public IActionResult Get(Guid id)
        {
            var exercise = exercises.Get(id);
            if (exercise == null) 
            {
                return NotFound();
            }
            return Ok(ExerciseModel.ToModel(exercise));
        }

        protected override void Dispose(bool disposing) 
        {
            try {
                base.Dispose(disposing);
            } finally {
                exercises.Dispose();
            }
        }
    }
}