using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Safes;
using Salesinvoice.DTOs;
using Shared.Enums;

namespace Salesinvoice
{
    public class SalesinvoicesOperationsRepo : ISalesinvoicesOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<SalesinvoicesHeader> _salesinvoicesHeaderEntity;
        private ISafeOperationsRepo _safeOperationsRepo;
        public SalesinvoicesOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo)
        {
            this.context = context;
            _salesinvoicesHeaderEntity = context.Set<SalesinvoicesHeader>();
            this._safeOperationsRepo = safeOperationsRepo;
        }

        public IEnumerable<SalesinvoicesHeader> GetAll()
        {
            return _salesinvoicesHeaderEntity.Include("SalesinvoicesDetialsList").AsEnumerable().OrderByDescending(x => x.Id);
        }

        public SalesinvoiceListDTO GetAll(int currentPage, string keyword, bool isToday)
        {
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
            }

            return new SalesinvoiceListDTO()
            {
                Total = _salesinvoicesHeaderEntity.Count(),
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

        public bool Add(SalesinvoicesHeader salesinvoicesHeader)
        {
            SalesinvoicesHeader exsistedSalesHeader = GetSalesinvoiceHeaderByDateAndSellerId(salesinvoicesHeader.SalesinvoicesDate, salesinvoicesHeader.SellerId);
            if (exsistedSalesHeader == null)
            {
                context.Entry(salesinvoicesHeader).State = EntityState.Added;
                context.SaveChanges();
            }
            if (salesinvoicesHeader.Id == 0) salesinvoicesHeader.Id = exsistedSalesHeader.Id;
            SetSalesinvoicesHeaderId(salesinvoicesHeader.Id, salesinvoicesHeader.SalesinvoicesDetialsList);
            AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList);
            return true;
        }

        public bool Update(SalesinvoicesHeader salesinvoicesHeader)
        {
            context.Entry(salesinvoicesHeader).State = EntityState.Modified;
            context.SaveChanges();
            DeleteSalesinvoicesDetials(salesinvoicesHeader.Id);
            SetSalesinvoicesHeaderId(salesinvoicesHeader.Id, salesinvoicesHeader.SalesinvoicesDetialsList);
            AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList);
            return true;
        }

        public bool Delete(long id)
        {
            SalesinvoicesHeader salesinvoicesHeader = GetById(id);
            DeleteSalesinvoicesDetials(id);
            _salesinvoicesHeaderEntity.Remove(salesinvoicesHeader);
            context.SaveChanges();
            return true;
        }
        public void DeleteSalesinvoiceDetails(OrderHeader orderHeader)
        {
            List<SalesinvoicesDetials> salesinvoicesDetialsList = context.SalesinvoicesDetials.Where(x => x.OrderDate == orderHeader.Created).ToList();
            context.SalesinvoicesDetials.RemoveRange(salesinvoicesDetialsList);
            context.SaveChanges();
        }

        public void DeleteSalesinvoiceHeader(OrderHeader orderHeader)
        {
            List<SalesinvoicesHeader> salesinvoicesHeaderList = context.SalesinvoicesHeaders.Where(x => x.Created.ToShortDateString() == orderHeader.Created.ToShortDateString()).ToList();
            foreach (SalesinvoicesHeader salesinvoicesHeader in salesinvoicesHeaderList)
            {
                List<SalesinvoicesDetials> salesinvoices = context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == salesinvoicesHeader.Id).ToList();
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

        private void AddSalesinvoicesDetials(IEnumerable<SalesinvoicesDetials> salesinvoicesDetialsList)
        {
            context.SalesinvoicesDetials.AddRange(salesinvoicesDetialsList);
            context.SaveChanges();
        }

        private void DeleteSalesinvoicesDetials(long headerId)
        {
            IEnumerable<SalesinvoicesDetials> purchaseDetails = context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == headerId).AsEnumerable();
            context.SalesinvoicesDetials.RemoveRange(purchaseDetails);
            context.SaveChanges();
        }

        private SalesinvoicesHeader GetSalesinvoiceHeaderByDateAndSellerId(DateTime salesinvoicesDate, long sellerId)
        {
            return context.SalesinvoicesHeaders.FirstOrDefault(x => x.SalesinvoicesDate.ToShortDateString() == salesinvoicesDate.ToShortDateString() && x.SellerId == sellerId);
        }




        #endregion


    }
}
