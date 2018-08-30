using System;
using System.Collections.Generic;
using Homeworks.DataAccess;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class ExerciseLogic : IDisposable
    {
        private ExerciseRepository repository;

        public ExerciseLogic() {
            repository = new ExerciseRepository(ContextFactory.GetNewContext());
        }

        public void Create(Exercise exercise) {
            repository.Add(exercise);
            repository.Save();
        }

        public void Remove(Guid id) {
            Exercise exercise = repository.Get(id);
            if (exercise == null) {
                throw new ArgumentException("Invalid guid");
            }
            repository.Remove(exercise);
            repository.Save();
        }

        public void Update(Guid id, Exercise exercise) {
            Exercise exerciseToUpdate = repository.Get(id);
            if (exercise == null) {
                throw new ArgumentException("Invalid guid");
            }
            exerciseToUpdate.Problem = exercise.Problem;
            exerciseToUpdate.Score = exercise.Score;
            repository.Update(exerciseToUpdate);
            repository.Save();
        }

        public Exercise Get(Guid id) {
            return repository.Get(id);
        }

        public IEnumerable<Exercise> GetAll() {
            return repository.GetAll();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    repository.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
