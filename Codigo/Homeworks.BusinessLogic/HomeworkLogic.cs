using System;
using System.Collections.Generic;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class HomeworkLogic : IHomeworkLogic
    {
        private IRepository<Homework> repositoryHome;
        private IRepository<Exercise> repositoryExer;

        public HomeworkLogic(IRepository<Homework> homeworks, IRepository<Exercise> exercises) {
            repositoryHome = homeworks;
            repositoryExer = exercises;
        }

        public Homework Create(Homework homework) {
            ThrowErrorIfItsInvalid(homework);
            repositoryHome.Add(homework);
            repositoryHome.Save();
            return homework;
        }

        public void Remove(Guid id) {
            Homework homework = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homework);
            repositoryHome.Remove(homework);
            repositoryHome.Save();
        }

        public Homework Update(Guid id, Homework homework) {
            Homework homeworkToUpdate = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homeworkToUpdate);
            homeworkToUpdate.Update(homework);
            repositoryHome.Update(homeworkToUpdate);
            repositoryHome.Save();
            return homeworkToUpdate;
        }

        public Exercise AddExercise(Guid id, Exercise exercise)
        {
            Homework homework = repositoryHome.Get(id);
            ThrowErrorIfItsNull(homework);
            homework.Exercises.Add(exercise);
            repositoryHome.Update(homework);
            repositoryHome.Save();
            return exercise;
        }

        public Homework Get(Guid id) 
        {
            return repositoryHome.Get(id);
        }

        public IEnumerable<Homework> GetAll() 
        {
            return repositoryHome.GetAll();
        }

        public void Dispose()
        {
            repositoryExer.Dispose();
            repositoryHome.Dispose();
        }

        private static void ThrowErrorIfItsNull(Homework homework)
        {
            if (homework == null)
            {
                throw new ArgumentException("Invalid guid");
            }
        }

        private void ThrowErrorIfItsInvalid(Homework homework) 
        {
            if (!homework.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
