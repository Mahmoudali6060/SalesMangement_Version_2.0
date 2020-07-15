using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Sellers.DTOs;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers
{
    public class OrderHeaderRepository : ISellerOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<Seller> sellerEntity;

        public OrderHeaderRepository(EntitiesDbContext context)
        {
            this.context = context;
            sellerEntity = context.Set<Seller>();
        }


        public IEnumerable<Seller> GetAll()
        {
            return sellerEntity.AsEnumerable();
        }

        public SellerListDTO GetAll(int currentPage, string keyword)
        {
            var list = sellerEntity
                //.Include("SalesinvoicesHeader")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            return new SellerListDTO()
            {
                Total = sellerEntity.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }


        public Seller GetById(long id)
        {
            return sellerEntity.SingleOrDefault(s => s.Id == id);
        }

        public long Add(Seller seller)
        {
            context.Entry(seller).State = EntityState.Added;
            context.SaveChanges();
            return seller.Id;
        }

        public bool Update(Seller seller)
        {
            context.Entry(seller).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            Seller seller = GetById(id);
            sellerEntity.Remove(seller);
            context.SaveChanges();
            return true;
        }

    }
}
