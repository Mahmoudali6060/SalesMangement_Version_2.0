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
        private EntitiesDbContext _context;
        private DbSet<OrderDetails> _sellerEntity;
        private DbSet<List<OrderDetails>> _sellerEntityList;

        public OrderDetailsOperationsRepo(EntitiesDbContext context)
        {
            this._context = context;
            _sellerEntity = context.Set<OrderDetails>();
            _sellerEntityList = context.Set<List<OrderDetails>>();
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

        public bool AddRange(IEnumerable<OrderDetails> orderDetailesList,EntitiesDbContext context )
        {
            context.OrderDetails.AddRange(orderDetailesList);
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
