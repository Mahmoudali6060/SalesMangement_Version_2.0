using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.IRepository
{
    public interface IOperationsRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(long id);
    }
}
