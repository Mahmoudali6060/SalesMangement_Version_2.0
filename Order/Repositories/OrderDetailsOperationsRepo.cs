using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Order.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Repositories
{
    public class OrderDetailsOperationsRepo : IOrderDetailsOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<OrderDetails> sellerEntity;
        private DbSet<List<OrderDetails>> sellerEntityList;

        public OrderDetailsOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            sellerEntity = context.Set<OrderDetails>();
            sellerEntityList = context.Set<List<OrderDetails>>();
        }

        public IEnumerable<OrderDetails> GetAll()
        {
            throw new NotImplementedException();
        }

        public OrderDetails GetById(long id)
        {
            throw new NotImplementedException();
        }

        public bool Add(OrderDetails entiy)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(IEnumerable<OrderDetails> orderDetailesList )
        {
            context.OrderDetails.AddRange(orderDetailesList);
            //context.Entry(orderDetailesList).State = EntityState.Added;
            context.SaveChanges();
            return true;
        }

        public bool Update(OrderDetails entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

    }
}
