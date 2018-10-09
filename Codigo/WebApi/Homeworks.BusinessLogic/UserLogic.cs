using System;
using System.Collections.Generic;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        private IRepository<User> repository;

        public UserLogic(IRepository<User> repository) {
            this.repository = repository;
        }

        public User Create(User user) 
        {
            ThrowErrorIfItsInvalid(user);
            repository.Add(user);
            repository.Save();
            return user;
        }

        public User Get(Guid id) {
            return repository.Get(id);
        }

        public IEnumerable<User> GetAll() 
        {
            return repository.GetAll();
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        private void ThrowErrorIfItsInvalid(User user) 
        {
            if (user == null || !user.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
