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
        IEnumerable<OrderHeader> GetAllDaily();
        OrderListDTO GetAll(int currentPage, string keyword,bool isToday);
        OrderHeader GetById(long id);
        bool Add(OrderHeader entity);
        bool Update(OrderHeader entity);
        bool Delete(long id);
        bool DeleteRelatedOrderDetials(long headerId);

    }
}
