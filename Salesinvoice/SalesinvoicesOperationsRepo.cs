using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Safes;
using Salesinvoice.DTOs;
using Shared.Classes;
using Shared.Enums;

namespace Salesinvoice
{
    public class SalesinvoicesOperationsRepo : ISalesinvoicesOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<SalesinvoicesHeader> _salesinvoicesHeaderEntity;
        private ISafeOperationsRepo _safeOperationsRepo;
        public SalesinvoicesOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo)
        {
            this._context = context;
            _salesinvoicesHeaderEntity = context.Set<SalesinvoicesHeader>();
            this._safeOperationsRepo = safeOperationsRepo;
        }

        public IEnumerable<SalesinvoicesHeader> GetAll()
        {
            return _salesinvoicesHeaderEntity.Include("SalesinvoicesDetialsList").AsEnumerable().OrderByDescending(x => x.Id);
        }

        public SalesinvoiceListDTO GetAll(int currentPage, string keyword, bool isToday)
        {
            var total = 0;
            var list = _salesinvoicesHeaderEntity
                .Include("SalesinvoicesDetialsList")
                .Include("Seller")
                .OrderByDescending(x => x.Id)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Id.ToString().Contains(keyword) || x.Seller.Name.Contains(keyword) || x.SalesinvoicesDate.ToString("dd/MM/yyyy").Contains(keyword));
            }

            if (isToday)
            {
                list = list.Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
                total = _salesinvoicesHeaderEntity.Count(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
            }

            else
            {
                total = _salesinvoicesHeaderEntity.Count();
            }
            return new SalesinvoiceListDTO()
            {
                Total = total,
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }

        public IEnumerable<SalesinvoicesHeader> GetAllDaily()
        {
            return _salesinvoicesHeaderEntity.Include("SalesinvoicesDetialsList").AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.Id);
        }

        public SalesinvoicesHeader GetById(long id)
        {
            return _salesinvoicesHeaderEntity.SingleOrDefault(s => s.Id == id);
        }

        public bool Add(SalesinvoicesHeader salesinvoicesHeader, long orderHeaderId)
        {
            SalesinvoicesHeader exsistedSalesHeader = GetSalesinvoiceHeaderByDateAndSellerId(salesinvoicesHeader.SalesinvoicesDate, salesinvoicesHeader.SellerId);
            if (exsistedSalesHeader == null)
            {
                _context.Entry(salesinvoicesHeader).State = EntityState.Added;
                _context.SaveChanges();
            }
            if (salesinvoicesHeader.Id == 0) salesinvoicesHeader.Id = exsistedSalesHeader.Id;
            SetSalesinvoicesHeaderId(salesinvoicesHeader.Id, salesinvoicesHeader.SalesinvoicesDetialsList);
            AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList, orderHeaderId);
            return true;
        }

        public bool Update(SalesinvoicesHeader salesinvoicesHeader, long orderHeaderId)
        {
            _context.Entry(salesinvoicesHeader).State = EntityState.Modified;
            _context.SaveChanges();
            DeleteSalesinvoicesDetials(salesinvoicesHeader.Id);
            SetSalesinvoicesHeaderId(salesinvoicesHeader.Id, salesinvoicesHeader.SalesinvoicesDetialsList);
            AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList, orderHeaderId);
            return true;
        }

        public bool Update(SalesinvoicesHeader salesinvoicesHeader)
        {
            _context.Entry(salesinvoicesHeader).State = EntityState.Modified;
            _context.SaveChanges();
            _safeOperationsRepo.UpdateByHeaderId(salesinvoicesHeader.Id, salesinvoicesHeader.Total, AccountTypesEnum.Sellers);
            return true;
        }

        public bool Delete(long id)
        {
            SalesinvoicesHeader salesinvoicesHeader = GetById(id);
            DeleteSalesinvoicesDetials(id);
            _salesinvoicesHeaderEntity.Remove(salesinvoicesHeader);
            _context.SaveChanges();
            return true;
        }
        public void DeleteSalesinvoiceDetails(OrderHeader orderHeader)
        {
            List<SalesinvoicesDetials> salesinvoicesDetialsList = _context.SalesinvoicesDetials.Where(x => x.OrderDate == orderHeader.Created).ToList();
            _context.SalesinvoicesDetials.RemoveRange(salesinvoicesDetialsList);
            _context.SaveChanges();
        }

        public void DeleteSalesinvoiceHeader(OrderHeader orderHeader)
        {
            List<SalesinvoicesHeader> salesinvoicesHeaderList = _context.SalesinvoicesHeaders.Where(x => x.Created.ToShortDateString() == orderHeader.Created.ToShortDateString()).ToList();
            foreach (SalesinvoicesHeader salesinvoicesHeader in salesinvoicesHeaderList)
            {
                List<SalesinvoicesDetials> salesinvoices = _context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == salesinvoicesHeader.Id).ToList();
                if (salesinvoices.Count == 0)
                    Delete(salesinvoicesHeader.Id);
            }
        }

        #region Helper
        private IEnumerable<SalesinvoicesDetials> SetSalesinvoicesHeaderId(long salesinvoicesHeaderId, IEnumerable<SalesinvoicesDetials> salesinvoicesDetails)
        {
            foreach (var item in salesinvoicesDetails)
            {
                item.SalesinvoicesHeaderId = salesinvoicesHeaderId;
                item.Id = 0;
            }
            return salesinvoicesDetails;
        }

        private void AddSalesinvoicesDetials(IEnumerable<SalesinvoicesDetials> salesinvoicesDetialsList, long orderHeaderId)
        {
            _context.SalesinvoicesDetials.AddRange(salesinvoicesDetialsList);
            UpdateSalesinvoiceTotal(salesinvoicesDetialsList, orderHeaderId);
            _context.SaveChanges();
        }

        private void UpdateSalesinvoiceTotal(IEnumerable<SalesinvoicesDetials> salesinvoicesDetialsList, long orderHeaderId)
        {
            decimal total = 0;
            decimal mashalTotal = 0;
            decimal byaaTotal = 0;

            long salesHeaderId = 0;
            foreach (SalesinvoicesDetials item in salesinvoicesDetialsList)
            {
                salesHeaderId = item.SalesinvoicesHeaderId;
                total += (item.Weight * item.Price) + (AppSettings.MashalRate + AppSettings.ByaaRate) * item.Quantity;
                mashalTotal += AppSettings.MashalRate * item.Quantity;
                byaaTotal += AppSettings.ByaaRate * item.Quantity;
            }
            SalesinvoicesHeader salesinvoicesHeader = _context.SalesinvoicesHeaders.SingleOrDefault(x => x.Id == salesHeaderId);
            salesinvoicesHeader.Total = salesinvoicesHeader.Total + total;
            salesinvoicesHeader.MashalTotal = salesinvoicesHeader.MashalTotal + mashalTotal;
            salesinvoicesHeader.ByaaTotal = salesinvoicesHeader.ByaaTotal + byaaTotal;

            _context.SaveChanges();

            _safeOperationsRepo.DeleteByHeaderId(salesinvoicesHeader.Id, AccountTypesEnum.Sellers);
            var sellerSafe = PrepareSellerSafeEntity(salesinvoicesHeader, salesinvoicesHeader.Total, orderHeaderId);
            _safeOperationsRepo.Add(sellerSafe);
        }

        private Safe PrepareSellerSafeEntity(SalesinvoicesHeader entity, decimal total, long orderId)
        {
            return new Safe()
            {
                Date = entity.SalesinvoicesDate,
                AccountId = entity.SellerId,
                AccountTypeId = (int)AccountTypesEnum.Sellers,
                Outcoming = total,
                Notes = $" رقم الكشف:{entity.Id}",
                IsHidden = true,
                HeaderId = entity.Id,
                OrderId = orderId
            };
        }


        private void DeleteSalesinvoicesDetials(long headerId)
        {
            IEnumerable<SalesinvoicesDetials> purchaseDetails = _context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == headerId).AsEnumerable();
            _context.SalesinvoicesDetials.RemoveRange(purchaseDetails);
            _context.SaveChanges();
        }

        private SalesinvoicesHeader GetSalesinvoiceHeaderByDateAndSellerId(DateTime salesinvoicesDate, long sellerId)
        {
            return _context.SalesinvoicesHeaders.FirstOrDefault(x => x.SalesinvoicesDate.ToShortDateString() == salesinvoicesDate.ToShortDateString() && x.SellerId == sellerId);
        }




        #endregion


    }
}
