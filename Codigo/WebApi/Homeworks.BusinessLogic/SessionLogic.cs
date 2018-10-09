using System;
using System.Linq;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class SessionLogic : ISessionLogic
    {
        // TENDRIA QUE SER UN SESSION REPOSITORY
        // SESSION = {
        //      token: Guid,  
        //      user: User
        // }
        private IRepository<User> repository;

        public SessionLogic(IRepository<User> repository) {
            this.repository = repository;
        }

        public bool IsValidToken(string token)
        {
            // SI EL TOKEN EXISTE EN BD RETORNA TRUE
            return true;
        }

        public Guid CreateToken(string userName, string password)
        {
            // SI EL USUARIO EXISTE Y LA PASS Y EL USERNAME SON EL MISMO
            // RETORANR GUID
            return Guid.NewGuid();
        }

        public bool HasLevel(string token, string role)
        {  
            // SI EL DUENIO DEL TOKEN TIENE EL ROLE REQUERIDO
            // RETORNA TRUE
            return true;
        }

        public User GetUser(string token) 
        {
            return repository.GetAll().ToList()[0];
        }

        public void Dispose()
        {
            repository.Dispose();
        }
    }
}
