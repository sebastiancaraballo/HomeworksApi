using Homeworks.Domain;
using System;
using System.Collections.Generic;

namespace Homeworks.BusinessLogic.Interface
{
    public interface IExerciseLogic : IDisposable
    {
        void Create(Exercise exercise);

        void Remove(Guid id);

        void Update(Guid id, Exercise exercise);

        Exercise Get(Guid id);

        IEnumerable<Exercise> GetAll();
    }
}
