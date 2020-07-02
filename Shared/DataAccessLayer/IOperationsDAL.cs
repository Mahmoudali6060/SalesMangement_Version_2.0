using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DataAccessLayer
{
   public interface IOperationsDAL<T>
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        void Update(T entity);
        void Add(T entity);
        void Delete(long id);
    }
}
