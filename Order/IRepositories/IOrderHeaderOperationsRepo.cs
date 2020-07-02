using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.IRepositories
{
    public interface IOrderHeaderOperationsRepo
    {
        IEnumerable<OrderHeader> GetAll();
        IEnumerable<OrderHeader> GetAllDaily();
        OrderHeader GetById(long id);
        bool Add(OrderHeader entity);
        bool Update(OrderHeader entity);
        bool Delete(long id);
        bool DeleteRelatedOrderDetials(long headerId);

    }
}
