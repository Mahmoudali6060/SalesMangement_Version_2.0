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
        private EntitiesDbContext _context;
        private DbSet<Order_Purechase> _order_purechaseEntity;

        public Order_PurechaseOperationsRepo(EntitiesDbContext context)
        {
            this._context = context;
            _order_purechaseEntity = context.Set<Order_Purechase>();
        }

        public IEnumerable<Order_Purechase> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order_Purechase GetById(long id)
        {
            throw new NotImplementedException();
        }

        public bool Add(Order_Purechase entity, EntitiesDbContext context)
        {
            context.Order_Purechases.Add(entity);
            context.SaveChanges();
            return true;
        }

        public bool Update(Order_Purechase entity)
        {
            Order_Purechase order_Purechase = _context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == entity.OrderHeaderId);
            if (order_Purechase != null)
                order_Purechase.PurechasesHeaderId = entity.PurechasesHeaderId;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(long orderHeaderId)
        {
            Order_Purechase order_Purechase = _context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
            _order_purechaseEntity.Remove(order_Purechase);
            _context.SaveChanges();
            return true;
        }

        public Order_Purechase GetByOrderHeaderId(long orderHeaderId)
        {
            return _context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
        }
    }
}
