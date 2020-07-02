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
    public class OrderHeaderOperationsRepo : IOrderHeaderOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<OrderHeader> orderHeaderEntity;

        public OrderHeaderOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            orderHeaderEntity = context.Set<OrderHeader>();
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            return context.OrderHeaders.Include("OrderDetails").Include("Farmer").ToList().OrderBy(x => x.Id);
        }
        public IEnumerable<OrderHeader> GetAllDaily()
        {
            return context.OrderHeaders.Include("OrderDetails").Include("Farmer").ToList().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderBy(x => x.Id);
        }
        public OrderHeader GetById(long id)
        {
            return orderHeaderEntity.Include("OrderDetails").Include("Farmer").SingleOrDefault(s => s.Id == id);
        }

        public bool Add(OrderHeader entity)
        {
            context.OrderHeaders.Add(entity);
            context.SaveChanges();
            return true;
        }

        public bool Update(OrderHeader entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            OrderHeader OrderHeader = GetById(id);
            orderHeaderEntity.Remove(OrderHeader);
            context.SaveChanges();
            return true;
        }

        public bool DeleteRelatedOrderDetials(long headerId)
        {
            IEnumerable<OrderDetails> orderDetails = context.OrderDetails.Where(x => x.OrderHeaderId == headerId);
            context.OrderDetails.RemoveRange(orderDetails);
            context.SaveChanges();
            return true;
        }

       
    }
}
