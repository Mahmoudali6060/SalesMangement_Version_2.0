using Database.Entities;
using Shared.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Safes
{
   public interface ISafeOperationsRepo 
    {
        IEnumerable<Safe> GetAll();
        Safe GetById(long id);
        long Add(Safe entity);
        bool Update(Safe entity);
        bool Delete(long id);
    }
}
