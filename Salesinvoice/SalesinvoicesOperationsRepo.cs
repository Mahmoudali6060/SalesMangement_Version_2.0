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
                list = list.Where(x => x.SalesinvoicesDate.ToShortDateString() == DateTime.Now.ToShortDateString());
                total = list.Count(x => x.SalesinvoicesDate.ToShortDateString() == DateTime.Now.ToShortDateString());
            }

            else
            {
                total = list.Count();
            }
            return new SalesinvoiceListDTO()
            {
                Total = total,
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }
        public SalesinvoicesHeader GetById(long id)
        {
            return _salesinvoicesHeaderEntity.SingleOrDefault(s => s.Id == id);
        }
        public SalesinvoicesHeader GetById(long id, EntitiesDbContext context)
        {
            return context.SalesinvoicesHeaders.SingleOrDefault(s => s.Id == id);
        }
        public SalesinvoicesHeader Add(SalesinvoicesHeader salesinvoicesHeader, long orderHeaderId, EntitiesDbContext context)
        {
            //To-Do
            //Check if Seller has at least one salesinvoice at this day or no
            SalesinvoicesHeader exsistedSalesHeader = GetSalesinvoiceHeaderByDateAndSellerId(salesinvoicesHeader.SalesinvoicesDate, salesinvoicesHeader.SellerId, context);
            if (exsistedSalesHeader == null)//It is the first salesinvoice to this Seller in this day
            {
                context.Entry(salesinvoicesHeader).State = EntityState.Added;
                context.SaveChanges();
            }
            if (salesinvoicesHeader.Id == 0) salesinvoicesHeader.Id = exsistedSalesHeader.Id;

            SetSalesinvoicesHeaderId(salesinvoicesHeader, salesinvoicesHeader.SalesinvoicesDetialsList);//Set SalesinvoiceHeaderId
            AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList, context);//Add SalesinvoiceDetails
            return UpdateSalesinvoiceTotal(salesinvoicesHeader.SalesinvoicesDetialsList, context);//Return updated salesinvoiceHeader
        }



        public bool Update(SalesinvoicesHeader salesinvoicesHeader, EntitiesDbContext context)
        {
            context.SalesinvoicesHeaders.Update(salesinvoicesHeader);
            context.SaveChanges();

            //DeleteSalesinvoicesDetials(salesinvoicesHeader.Id, context);
            //SetSalesinvoicesHeaderId(salesinvoicesHeader, salesinvoicesHeader.SalesinvoicesDetialsList);
            //AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList, context);
            //UpdateSalesinvoiceTotal(salesinvoicesHeader.SalesinvoicesDetialsList, context);
            return true;
        }
        public bool Update(SalesinvoicesHeader salesinvoicesHeader)
        {
            _context.SalesinvoicesHeaders.Update(salesinvoicesHeader);
            _context.SaveChanges();

            //DeleteSalesinvoicesDetials(salesinvoicesHeader.Id, _context);
            //SetSalesinvoicesHeaderId(salesinvoicesHeader, salesinvoicesHeader.SalesinvoicesDetialsList);
            //AddSalesinvoicesDetials(salesinvoicesHeader.SalesinvoicesDetialsList, _context);
            //UpdateSalesinvoiceTotal(salesinvoicesHeader.SalesinvoicesDetialsList, _context);
            return true;
        }

        public bool UpdateInPrinting(SalesinvoicesHeader entity)
        {
            entity.IsPrinted = true;
            return Update(entity);
        }
        public bool Delete(long id, EntitiesDbContext context)
        {
            SalesinvoicesHeader salesinvoicesHeader = GetById(id, context);
            DeleteSalesinvoicesDetials(id, context);
            context.SalesinvoicesHeaders.Remove(salesinvoicesHeader);
            context.SaveChanges();
            return true;
        }
        public List<SalesinvoicesDetials> DeleteSalesinvoiceDetails(OrderHeader orderHeader, EntitiesDbContext context)
        {
            List<SalesinvoicesDetials> salesinvoicesDetialsList = _context.SalesinvoicesDetials.Where(x => x.OrderDate == orderHeader.Created).ToList();
            context.SalesinvoicesDetials.RemoveRange(salesinvoicesDetialsList);
            context.SaveChanges();
            return salesinvoicesDetialsList;
        }
        public void DeleteSalesinvoiceHeader(DateTime orderHeaderCreatedDate, EntitiesDbContext context)
        {
            List<SalesinvoicesHeader> salesinvoicesHeaderList = context.SalesinvoicesHeaders.Where(x => x.Created.ToShortDateString() == orderHeaderCreatedDate.ToShortDateString()).ToList();
            foreach (SalesinvoicesHeader salesinvoicesHeader in salesinvoicesHeaderList)
            {
                List<SalesinvoicesDetials> salesinvoices = context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == salesinvoicesHeader.Id).ToList();
                if (salesinvoices.Count == 0)
                {
                    _safeOperationsRepo.DeleteByHeaderId(salesinvoicesHeader.Id, AccountTypesEnum.Sellers, context);//Delete old record in safe related to this Seller
                    Delete(salesinvoicesHeader.Id, context);
                }
                
            }
        }

        public IEnumerable<SalesinvoicesHeader> GetAllDaily(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (dateFrom == null && dateTo == null)
                return _salesinvoicesHeaderEntity.Include("SalesinvoicesDetialsList").AsEnumerable().Where(x => x.SalesinvoicesDate.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.Id);
            return _salesinvoicesHeaderEntity.Include("SalesinvoicesDetialsList").AsEnumerable().Where(x => x.SalesinvoicesDate.Date >= dateFrom.Value.Date && x.SalesinvoicesDate.Date <= dateTo.Value.Date).OrderByDescending(x => x.Id);
        }

        public bool DeleteByFarmerId(long farmerId, EntitiesDbContext context)
        {
            IEnumerable<SalesinvoicesDetials> SalesinvoicesDetialsList = context.SalesinvoicesDetials.Where(x => x.FarmerId == farmerId);
            foreach (var item in SalesinvoicesDetialsList)
            {
                context.SalesinvoicesDetials.Remove(item);
                RemoveEmptySalesInvoiceHeader(item.SalesinvoicesHeaderId, context);
                context.SaveChanges();
            }

            return true;
        }
        #region Helper
        private bool RemoveEmptySalesInvoiceHeader(long salesInvoiceHeaderId, EntitiesDbContext context)
        {
            IEnumerable<SalesinvoicesDetials> salesInvoiceDetails = context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == salesInvoiceHeaderId);
            if (salesInvoiceDetails.Count() == 1)
            {
                var salesInvoiceHeader = context.SalesinvoicesHeaders.SingleOrDefault(x => x.Id == salesInvoiceHeaderId);
                context.SalesinvoicesHeaders.Remove(salesInvoiceHeader);
                return true;
            }
            return true;
        }

        private IEnumerable<SalesinvoicesDetials> SetSalesinvoicesHeaderId(SalesinvoicesHeader salesinvoicesHeader, IEnumerable<SalesinvoicesDetials> salesinvoicesDetails)
        {
            foreach (var item in salesinvoicesDetails)
            {
                item.SalesinvoicesHeaderId = salesinvoicesHeader.Id;
                item.Id = 0;
            }
            return salesinvoicesDetails;
        }
        private void AddSalesinvoicesDetials(IEnumerable<SalesinvoicesDetials> salesinvoicesDetialsList, EntitiesDbContext context)
        {
            context.SalesinvoicesDetials.AddRange(salesinvoicesDetialsList);
            context.SaveChanges();
        }
        private SalesinvoicesHeader UpdateSalesinvoiceTotal(IEnumerable<SalesinvoicesDetials> salesinvoicesDetialsList, EntitiesDbContext context)
        {
            decimal total = 0;//Calculate Total
            decimal mashalTotal = 0;//Calculate Mashal Total
            decimal byaaTotal = 0;//Calculate Byaa Total
            long salesHeaderId = 0;

            foreach (SalesinvoicesDetials item in salesinvoicesDetialsList)
            {
                salesHeaderId = item.SalesinvoicesHeaderId;
                total += (item.Weight * item.Price) + (AppSettings.MashalRate + AppSettings.ByaaRate) * item.Quantity;
                mashalTotal += AppSettings.MashalRate * item.Quantity;
                byaaTotal += AppSettings.ByaaRate * item.Quantity;
            }
            SalesinvoicesHeader salesinvoicesHeader = context.SalesinvoicesHeaders.SingleOrDefault(x => x.Id == salesHeaderId);
            salesinvoicesHeader.Total = salesinvoicesHeader.Total + total;
            salesinvoicesHeader.MashalTotal = salesinvoicesHeader.MashalTotal + mashalTotal;
            salesinvoicesHeader.ByaaTotal = salesinvoicesHeader.ByaaTotal + byaaTotal;

            context.SaveChanges();
            return salesinvoicesHeader;
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
        private void DeleteSalesinvoicesDetials(long headerId, EntitiesDbContext context)
        {
            IEnumerable<SalesinvoicesDetials> purchaseDetails = context.SalesinvoicesDetials.Where(x => x.SalesinvoicesHeaderId == headerId).AsEnumerable();
            context.SalesinvoicesDetials.RemoveRange(purchaseDetails);
            context.SaveChanges();
        }
        private SalesinvoicesHeader GetSalesinvoiceHeaderByDateAndSellerId(DateTime salesinvoicesDate, long sellerId, EntitiesDbContext context)
        {
            return context.SalesinvoicesHeaders.FirstOrDefault(x => x.SalesinvoicesDate.ToShortDateString() == salesinvoicesDate.ToShortDateString() && x.SellerId == sellerId);
        }
        #endregion
    }
}
