using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Order.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.Repositories
{
    public class Order_PurechaseOperationsRepo : IOrder_PurechaseOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<Order_Purechase> order_purechaseEntity;

        public Order_PurechaseOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            order_purechaseEntity = context.Set<Order_Purechase>();
        }

        public IEnumerable<Order_Purechase> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order_Purechase GetById(long id)
        {
            throw new NotImplementedException();
        }

        public bool Add(Order_Purechase entity)
        {
            context.Order_Purechases.Add(entity);
            context.SaveChanges();
            return true;
        }

        public bool Update(Order_Purechase entity)
        {
            Order_Purechase order_Purechase = context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == entity.OrderHeaderId);
            if (order_Purechase != null)
                order_Purechase.PurechasesHeaderId = entity.PurechasesHeaderId;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long orderHeaderId)
        {
            Order_Purechase order_Purechase = context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
            order_purechaseEntity.Remove(order_Purechase);
            context.SaveChanges();
            return true;
        }

        public Order_Purechase GetByOrderHeaderId(long orderHeaderId)
        {
            return context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
        }
    }
}
