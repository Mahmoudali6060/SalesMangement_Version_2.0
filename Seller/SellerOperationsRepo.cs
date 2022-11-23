using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Safes;
using Safes.DTOs;
using Sellers.DTOs;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            //foreach (var seller in list)
            //{
            //    BalanceDTO balanceDTO = _safeOperationsRepo.GetBalanceByAccountId(seller.Id, AccountTypesEnum.Sellers);
            //    seller.Balance = balanceDTO.TotalOutcoming - balanceDTO.TotalIncoming;
            //}

            return new SellerListDTO()
            {
                Total = list.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
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
            Safe safe = PrepareSellerSafeEntity(seller);
            _safeOperationsRepo.Add(safe);
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

        public bool UpdateBalance(long sellerId, decimal balance, EntitiesDbContext context)
        {
            var exsitedSeller = GetById(sellerId);
            exsitedSeller.Balance += balance;
            context.Entry(exsitedSeller).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        private Safe PrepareSellerSafeEntity(Seller entity)
        {
            return new Safe()
            {
                Date = DateTime.Now.Date,
                AccountId = entity.Id,
                AccountTypeId = (int)AccountTypesEnum.Sellers,
                Outcoming = entity.Balance,
                Notes = $"رصيد افتتاحي",
                IsHidden = true,
                IsTransfered = true,
                HeaderId = 0,
                OrderId = 0
            };
        }

    }
}
