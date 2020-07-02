using Database.Entities;
using System.Collections.Generic;

namespace Sellers
{
   public interface ISellerOperationsRepo 
    {
        IEnumerable<Seller> GetAll();
        Seller GetById(long id);
        bool Add(Seller entity);
        bool Update(Seller entity);
        bool Delete(long id);
    }
}
