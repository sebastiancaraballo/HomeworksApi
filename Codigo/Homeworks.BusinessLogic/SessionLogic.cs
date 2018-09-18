using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.DataAccess;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class SessionLogic : IDisposable
    {
        // TENDRIA QUE SER UN SESSION REPOSITORY
        // SESSION = {
        //      token: Guid,  
        //      user: User
        // }
        private UserRepository repository;

        public SessionLogic() {
            repository = new UserRepository(ContextFactory.GetNewContext());
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
