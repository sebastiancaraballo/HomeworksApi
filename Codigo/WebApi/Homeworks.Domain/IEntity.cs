using System;
using System.Collections.Generic;

namespace Homeworks.Domain
{
    public interface IEntity<T>
    {
        // RETORNA SI LA ENTIDAD ES VALIDA, EN OTRO CASO FALSE
        // EJ: SI LA ENTIDAD TIENE UN CAMPO 
        // QUE NO DEBERIA SER NULL RETORNA FALSE
        bool IsValid();

        T Update(T entity);
    }
}
