using Database;
using Database.Entities;
using Sellers.DTOs;
using System.Collections.Generic;

namespace Sellers
{
   public interface ISellerOperationsRepo 
    {
        IEnumerable<Seller> GetAll();
        SellerListDTO GetAll(int currentPage, string keyword);
        Seller GetById(long id);
        long Add(Seller entity);
        bool Update(Seller entity);
        bool Delete(long id);
        bool UpdateBalance(long farmerId, decimal balance, EntitiesDbContext context);

    }
}
