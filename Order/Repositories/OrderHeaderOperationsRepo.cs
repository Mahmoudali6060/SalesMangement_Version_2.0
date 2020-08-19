using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Order.DTOs;
using Order.IRepositories;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.Repositories
{
    public class OrderHeaderOperationsRepo : IOrderHeaderOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<OrderHeader> _orderHeaderEntity;

        public OrderHeaderOperationsRepo(EntitiesDbContext context)
        {
            this._context = context;
            _orderHeaderEntity = context.Set<OrderHeader>();
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            return _context.OrderHeaders.Include("OrderDetails").Include("Farmer").ToList().OrderBy(x => x.Id);
        }
        public OrderListDTO GetAll(int currentPage, string keyword, bool isToday)
        {
            var total = 0;
            var list = _orderHeaderEntity
                .Include("OrderDetails").Include("Farmer")
                .OrderBy(x => x.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Id.ToString().Contains(keyword) || x.Farmer.Name.Contains(keyword) || x.OrderDate.ToString("dd/MM/yyyy").Contains(keyword));
            }

            if (isToday)
            {
                list = list.Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
                total = list.Count(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
            }
            else
            {
                total = _orderHeaderEntity.Count();
            }

            return new OrderListDTO()
            {
                Total = total,
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }
        public OrderHeader GetById(long id)
        {
            return _orderHeaderEntity.Include("OrderDetails").Include("Farmer").SingleOrDefault(s => s.Id == id);
        }
        public OrderHeader GetById(long id, EntitiesDbContext context)
        {
            return context.OrderHeaders.Include("OrderDetails").Include("Farmer").SingleOrDefault(s => s.Id == id);
        }

        public bool Add(OrderHeader entity, EntitiesDbContext context)
        {
            context.OrderHeaders.Add(entity);
            context.SaveChanges();
            return true;
        }

        public bool Update(OrderHeader entity, EntitiesDbContext context)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id, EntitiesDbContext context)
        {
            OrderHeader OrderHeader = GetById(id, context);
            context.OrderHeaders.Remove(OrderHeader);
            context.SaveChanges();
            return true;
        }

        public bool DeleteRelatedOrderDetials(long headerId, EntitiesDbContext context)
        {
            IEnumerable<OrderDetails> orderDetails = context.OrderDetails.Where(x => x.OrderHeaderId == headerId);
            context.OrderDetails.RemoveRange(orderDetails);
            context.SaveChanges();
            return true;
        }


    }
}
