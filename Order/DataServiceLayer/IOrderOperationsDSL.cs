using Database.Entities;
using Order.DTOs;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.DataServiceLayer
{
   public interface IOrderOperationsDSL
    {
        IEnumerable<OrderHeader> GetAll();
        OrderListDTO GetAll(int currentPage, string keyword,bool isToday);
        OrderHeader GetById(long id);
        bool Add(OrderDTO entity);
        bool Update(OrderDTO entity);
        bool Delete(long id);
    }
}
