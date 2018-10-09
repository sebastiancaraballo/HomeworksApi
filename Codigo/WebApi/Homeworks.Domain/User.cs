using System;
using System.Collections.Generic;

namespace Homeworks.Domain
{
    public class User : IEntity<User>
    {       
        public Guid Id {get; set;}
        public string Name {get; set;}
        public string UserName {get; set;}
        public string Password {get; set;}

        public bool IsValid()
        {
            return true;
        }

        public User Update(User entity)
        {
            if (entity.Name != null)
                Name = entity.Name;
            if (entity.Password != null)
                Password = entity.Password;
            return this;
        }
    }
}
