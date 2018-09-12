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
    public class ExercisesController : Controller
    {
        private ExerciseLogic exercises;

        public ExercisesController() 
        {
            exercises = new ExerciseLogic();
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