using Database;
using Database.Entities;
using Order.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.IRepositories
{
    public interface IOrderHeaderOperationsRepo
    {
        IEnumerable<OrderHeader> GetAll();
        OrderListDTO GetAll(int currentPage, string keyword, bool isToday);
        OrderHeader GetById(long id, EntitiesDbContext context);
        OrderHeader GetById(long id);
        bool Add(OrderHeader entity, EntitiesDbContext context);
        bool Update(OrderHeader entity, EntitiesDbContext context);
        bool Delete(long id, EntitiesDbContext context);
        bool DeleteRelatedOrderDetials(long headerId, EntitiesDbContext context);

    }
}
