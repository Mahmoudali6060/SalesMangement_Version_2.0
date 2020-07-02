using Database.Entities;
using Shared.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmers
{
   public interface IFarmerOperationsRepo 
    {
        IEnumerable<Farmer> GetAll();
        Farmer GetById(long id);
        long Add(Farmer entity);
        bool Update(Farmer entity);
        bool Delete(long id);
    }
}
