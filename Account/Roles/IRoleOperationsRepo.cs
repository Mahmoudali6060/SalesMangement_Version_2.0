using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Roles
{
   public interface IRoleOperationsRepo 
    {
        IEnumerable<Role> GetAll();
        Role GetById(long id);
        bool Add(Role entity);
        bool Update(Role entity);
        bool Delete(long id);
    }
}
