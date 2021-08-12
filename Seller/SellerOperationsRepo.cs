using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Safes;
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
        private EntitiesDbContext _context;
        private DbSet<Seller> _sellerEntity;
        private readonly ISafeOperationsRepo _safeOperationsRepo;

        public OrderHeaderRepository(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo)
        {
            _context = context;
            _sellerEntity = context.Set<Seller>();
            _safeOperationsRepo = safeOperationsRepo;
        }

        public IEnumerable<Seller> GetAll()
        {
            return _sellerEntity.AsEnumerable().OrderBy(x => x.Name);
        }

        public SellerListDTO GetAll(int currentPage, string keyword)
        {
            var list = _sellerEntity
                //.Include("SalesinvoicesHeader")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            return new SellerListDTO()
            {
                Total = list.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize).OrderBy(x => x.Name)
            };
        }


        public Seller GetById(long id)
        {
            return _sellerEntity.SingleOrDefault(s => s.Id == id);
        }

        public long Add(Seller seller)
        {
            seller.Id = 0;
            _context.Entry(seller).State = EntityState.Added;
            _context.SaveChanges();
            return seller.Id;
        }

        public bool Update(Seller seller)
        {
            _context.Entry(seller).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            _safeOperationsRepo.DeleteByAccountId(id, AccountTypesEnum.Sellers);//Delete related record in Safe table
            Seller seller = GetById(id);
            _sellerEntity.Remove(seller);
            _context.SaveChanges();
            return true;
        }
    }
}
