using System;
using System.Collections.Generic;
using Homeworks.DataAccess;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class HomeworkLogic : IDisposable
    {
        private HomeworkRepository repositoryHome;
        private ExerciseRepository repositoryExer;

        public HomeworkLogic() {
            var context = ContextFactory.GetNewContext();
            repositoryHome = new HomeworkRepository(context);
            repositoryExer = new ExerciseRepository(context);
        }

        public Homework Create(Homework homework) {
            repositoryHome.Add(homework);
            repositoryHome.Save();
            return homework;
        }

        public void Remove(Guid id) {
            Homework homework = repositoryHome.Get(id);
            if (homework == null) {
                throw new ArgumentException("Invalid guid");
            }
            repositoryHome.Remove(homework);
            repositoryHome.Save();
        }

        public Homework Update(Guid id, Homework homework) {
            Homework homeworkToUpdate = repositoryHome.Get(id);
            if (homeworkToUpdate == null) {
                throw new ArgumentException("Invalid guid");
            }
            homeworkToUpdate.Description = homework.Description;
            homeworkToUpdate.DueDate = homework.DueDate;
            repositoryHome.Update(homeworkToUpdate);
            repositoryHome.Save();
            return homeworkToUpdate;
        }

        public Exercise AddExercise(Guid id, Exercise exercise)
        {
            Homework homework = repositoryHome.Get(id);
            if (homework == null) {
                throw new ArgumentException("Invalid guid");
            }
            homework.Exercises.Add(exercise);
            repositoryHome.Update(homework);
            repositoryHome.Save();
            return exercise;
        }

        public Homework Get(Guid id) {
            return repositoryHome.Get(id);
        }

        public IEnumerable<Homework> GetAll() {
            return repositoryHome.GetAll();
        }

        public void Dispose()
        {
            repositoryExer.Dispose();
            repositoryHome.Dispose();
        }
    }
}
