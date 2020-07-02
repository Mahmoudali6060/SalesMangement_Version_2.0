using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.IRepositories
{
    public interface IOrderDetailsOperationsRepo
    {
        IEnumerable<OrderDetails> GetAll();
        OrderDetails GetById(long id);
        bool Add(OrderDetails entity);
        bool AddRange(IEnumerable<OrderDetails> orderDetailesList);
        bool Update(OrderDetails entity);
        bool Delete(long id);

    }
}
