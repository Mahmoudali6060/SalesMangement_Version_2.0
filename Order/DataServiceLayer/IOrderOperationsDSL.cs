using Database.Entities;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.DataServiceLayer
{
   public interface IOrderOperationsDSL
    {
        IEnumerable<OrderHeader> GetAll();
        IEnumerable<OrderHeader> GetAllDaily();

        OrderHeader GetById(long id);
        bool Add(OrderDTO entity);
        bool Update(OrderDTO entity);
        bool Delete(long id);
    }
}
