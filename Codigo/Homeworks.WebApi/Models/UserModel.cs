using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;

namespace Homeworks.WebApi.Models
{
    public class UserModel : Model<User, UserModel>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public UserModel() { }

        public UserModel(User entity)
        {
            SetModel(entity);
        }

        public override User ToEntity() => new User()
        {
            Id = this.Id,
            Name = this.Name,
            UserName = this.UserName,
            Password = this.Password,
        };

        protected override UserModel SetModel(User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            UserName = entity.UserName;
            return this;
        }

    }
}
