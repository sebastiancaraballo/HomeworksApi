using System;
using System.Collections.Generic;

namespace Homeworks.DataAccess.Interface
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T entity);

        void Remove(T entity);

        void Update(T entity);

        IEnumerable<T> GetAll();

        T Get(Guid id);

        void Save();
    }
}
