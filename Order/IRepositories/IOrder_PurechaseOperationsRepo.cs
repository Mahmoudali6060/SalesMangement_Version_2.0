using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.IRepositories
{
    public interface IOrder_PurechaseOperationsRepo
    {
        IEnumerable<Order_Purechase> GetAll();
        Order_Purechase GetById(long id);
        bool Add(Order_Purechase entity);
        bool Update(Order_Purechase entity);
        bool Delete(long id);
        Order_Purechase GetByOrderHeaderId(long orderHeaderId);
    }
}
