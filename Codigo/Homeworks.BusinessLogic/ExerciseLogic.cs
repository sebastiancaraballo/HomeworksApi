using System;
using System.Collections.Generic;
using Homeworks.DataAccess;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class ExerciseLogic : IDisposable
    {
        private ExerciseRepository repository;

        public ExerciseLogic() 
        {
            repository = new ExerciseRepository(ContextFactory.GetNewContext());
        }

        public void Create(Exercise exercise) 
        {
            ThrowErrorIfItsInvalid(exercise);
            repository.Add(exercise);
            repository.Save();
        }

        public void Remove(Guid id) 
        {
            Exercise exercise = repository.Get(id);
            ThrowErrorIfItsNull(exercise);
            repository.Remove(exercise);
            repository.Save();
        }

        public void Update(Guid id, Exercise exercise)
        {
            ThrowErrorIfItsInvalid(exercise);
            Exercise exerciseToUpdate = repository.Get(id);
            ThrowErrorIfItsNull(exerciseToUpdate);
            exerciseToUpdate.Update(exercise);
            repository.Update(exerciseToUpdate);
            repository.Save();
        }

        public Exercise Get(Guid id) 
        {
            return repository.Get(id);
        }

        public IEnumerable<Exercise> GetAll() 
        {
            return repository.GetAll();
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        private static void ThrowErrorIfItsNull(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentException("Invalid guid");
            }
        }

        private void ThrowErrorIfItsInvalid(Exercise exercise) 
        {
            if (!exercise.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
