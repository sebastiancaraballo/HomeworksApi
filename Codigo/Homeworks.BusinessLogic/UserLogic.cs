using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.DataAccess;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class UserLogic : IDisposable
    {
        private UserRepository repository;

        public UserLogic() {
            repository = new UserRepository(ContextFactory.GetNewContext());
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
            if (!user.IsValid()) 
            {
                throw new ArgumentException("Lanza error por que es invaldia la entity");
            }
        }
    }
}
