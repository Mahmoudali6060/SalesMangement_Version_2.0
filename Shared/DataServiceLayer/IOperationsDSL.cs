using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DataServiceLayer
{
   public interface IOperationsDSL<T>
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        bool Update(T entity);
        bool Add(T entity);
        bool Delete(long id);
    }
}
