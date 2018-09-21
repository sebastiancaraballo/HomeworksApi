using System;
using System.Collections.Generic;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic.Interface
{
    public interface IHomeworkLogic : IDisposable
    {
        Homework Create(Homework homework);

        void Remove(Guid id);

        Homework Update(Guid id, Homework homework);

        Exercise AddExercise(Guid id, Exercise exercise);

        Homework Get(Guid id);

        IEnumerable<Homework> GetAll();
    }
}
