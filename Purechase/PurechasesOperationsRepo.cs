using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Purechase.DTOs;
using Safes;
using Salesinvoice;
using Shared.Enums;

namespace Purechase
{
    public class PurechasesOperationsRepo : IPurechasesOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<PurechasesHeader> _purechasesHeaderEntity;
        private ISafeOperationsRepo _safeOperationsRepo;
        private ISalesinvoicesOperationsRepo _salesinvoicesOperationsRepo;

        public PurechasesOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo, ISalesinvoicesOperationsRepo salesinvoicesOperationsRepo)
        {
            this._context = context;
            _purechasesHeaderEntity = context.Set<PurechasesHeader>();
            _safeOperationsRepo = safeOperationsRepo;
            _salesinvoicesOperationsRepo = salesinvoicesOperationsRepo;
        }

        public IEnumerable<PurechasesHeader> GetAll()
        {
            return _purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().OrderByDescending(x => x.Id);
        }
        public PurechaseListDTO GetAll(int currentPage, string keyword, bool isToday)
        {
            var total = 0;
            var list = _purechasesHeaderEntity
                .Include("PurechasesDetialsList")
                .Include("Farmer")
                .OrderByDescending(x => x.Id)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Id.ToString().Contains(keyword) || x.Farmer.Name.Contains(keyword) || x.PurechasesDate.ToString("dd/MM/yyyy").Contains(keyword));
            }

            if (isToday)
            {
                list = list.Where(x => x.PurechasesDate.ToShortDateString() == DateTime.Now.ToShortDateString());
                total = list.Count(x => x.PurechasesDate.ToShortDateString() == DateTime.Now.ToShortDateString());
            }
            else
            {
                total = list.Count();
            }

            return new PurechaseListDTO()
            {
                Total = total,
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }
        public IEnumerable<PurechasesHeader> GetAllDaily(DateTime? date = null)
        {
            if (date == null)
                return _purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.Id);
            return _purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().Where(x => x.Created.ToShortDateString() == date.Value.ToShortDateString()).OrderByDescending(x => x.Id);
        }
        public PurechasesHeader GetById(long id)
        {
            return _purechasesHeaderEntity.SingleOrDefault(s => s.Id == id);
        }
        public bool Add(PurechasesHeader purechasesHeader, EntitiesDbContext context)
        {
            context.Entry(purechasesHeader).State = EntityState.Added;
            context.SaveChanges();
            SetPurechasesHeaderId(purechasesHeader.Id, purechasesHeader.PurechasesDetialsList);
            AddPurechasesDetials(purechasesHeader.PurechasesDetialsList, context);
            return true;
        }
        public bool Update(PurechasesHeader purechasesHeader, EntitiesDbContext context)
        {
            context.Entry(purechasesHeader).State = EntityState.Modified;
            context.SaveChanges();
            DeletePurchaseDetails(purechasesHeader.Id, context);
            SetPurechasesHeaderId(purechasesHeader.Id, purechasesHeader.PurechasesDetialsList);
            AddPurechasesDetials(purechasesHeader.PurechasesDetialsList, context);
            _safeOperationsRepo.UpdateByHeaderId(purechasesHeader.Id, purechasesHeader.Total, AccountTypesEnum.Clients, context);
            return true;
        }
        public bool Update(PurechasesHeader purechasesHeader)
        {
            _context.Entry(purechasesHeader).State = EntityState.Modified;
            _context.SaveChanges();
            DeletePurchaseDetails(purechasesHeader.Id);
            SetPurechasesHeaderId(purechasesHeader.Id, purechasesHeader.PurechasesDetialsList);
            AddPurechasesDetials(purechasesHeader.PurechasesDetialsList, _context);
            _safeOperationsRepo.UpdateByHeaderId(purechasesHeader.Id, purechasesHeader.Total, AccountTypesEnum.Clients);
            return true;
        }
        public bool Delete(long id, EntitiesDbContext context)
        {
            PurechasesHeader purechasesHeader = GetById(id);
            DeletePurchaseDetails(id);
            context.PurechasesHeaders.Remove(purechasesHeader);
            context.SaveChanges();
            return true;
        }
        public void DeleteRelatedPurechase(long orderHeaderId, EntitiesDbContext context)
        {
            Order_Purechase order_Purechase = _context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
            if (order_Purechase != null && order_Purechase.PurechasesHeaderId != 0)
            {
                PurechasesHeader purechasesHeader = _context.PurechasesHeaders.FirstOrDefault(x => x.Id == order_Purechase.PurechasesHeaderId);
                Delete(purechasesHeader.Id, context);
            }

        }
        public DashboardDTO GetDashboardData(DateTime? selectedDate = null)
        {
            var todayPurchases = GetAllDaily(selectedDate);
            var todaySalesinvoice = _salesinvoicesOperationsRepo.GetAllDaily(selectedDate);

            decimal total = CalculateTotalPurchase(todayPurchases);
            DashboardDTO dashboardDTO = new DashboardDTO()
            {
                //TotalPurchase = todayPurchases.Sum(x => Math.Ceiling(x.Total)),// + todayPurchases.Sum(x => x.Commission)+todayPurchases.Sum(x => x.Gift)+ todayPurchases.Sum(x => x.Descent)),
                TotalPurchase = total,
                TotalCommission = (todayPurchases.Sum(x => Math.Ceiling(x.Commission))),
                TotalGift = todayPurchases.Sum(x => Math.Ceiling(x.Gift)),
                TotalDescent = todayPurchases.Sum(x => Math.Ceiling(x.Descent)),

                TotalSalesinvoice = todaySalesinvoice.Sum(x => x.Total), //CalculateTotalSalesinvoice(todaySalesinvoice),
                TotalQuantity = todaySalesinvoice.Sum(x => x.SalesinvoicesDetialsList.Sum(y => y.Quantity)),
                TotalSalesWeight = todaySalesinvoice.Sum(x => x.SalesinvoicesDetialsList.Sum(y => y.Weight)),
                TotalPurchaseWeight = todayPurchases.Sum(x => x.PurechasesDetialsList.Sum(y => y.Weight))


            };
            //dashboardDTO.TotalPurchase += dashboardDTO.TotalCommission + dashboardDTO.TotalGift + dashboardDTO.TotalDescent;
            return dashboardDTO;
        }

        private decimal CalculateTotalPurchase(IEnumerable<PurechasesHeader> todayPurchases)
        {
            decimal total = 0;
            foreach (var purchase in todayPurchases)
            {
                foreach (var purchaseDetail in purchase.PurechasesDetialsList)
                {
                    decimal subTotal = 0;
                    subTotal = Math.Ceiling(purchaseDetail.Price * purchaseDetail.Weight);
                    total += subTotal;
                }
            }
            return total;
        }

        private decimal CalculateTotalSalesinvoice(IEnumerable<SalesinvoicesHeader> salesinvoicesHeaders)
        {
            decimal total = 0;
            foreach (var salesinvoice in salesinvoicesHeaders)
            {
                foreach (var salesinvoiceDetails in salesinvoice.SalesinvoicesDetialsList)
                {
                    decimal subTotal = 0;
                    subTotal = Math.Ceiling(salesinvoiceDetails.Price * salesinvoiceDetails.Weight);
                    total += subTotal;
                }
            }
            return total;
        }

        public IEnumerable<PurechasesHeader> GetPurchaseHeaderListByFarmerId(long farmerId)
        {
            return _purechasesHeaderEntity.Where(x => x.FarmerId == farmerId).Include("PurechasesDetialsList").AsEnumerable().OrderByDescending(x => x.PurechasesDate);
        }
        #region Helper
        private IEnumerable<PurechasesDetials> SetPurechasesHeaderId(long purechasesHeaderId, IEnumerable<PurechasesDetials> purechasesDetails)
        {
            foreach (var item in purechasesDetails)
            {
                item.PurechasesHeaderId = purechasesHeaderId;
                item.Id = 0;
            }
            return purechasesDetails;
        }
        private void AddPurechasesDetials(IEnumerable<PurechasesDetials> purechasesDetialsList, EntitiesDbContext context)
        {
            context.PurechasesDetials.AddRange(purechasesDetialsList);
            context.SaveChanges();
        }
        private void DeletePurchaseDetails(long headerId)
        {
            IEnumerable<PurechasesDetials> purchaseDetails = _context.PurechasesDetials.Where(x => x.PurechasesHeaderId == headerId);
            _context.PurechasesDetials.RemoveRange(purchaseDetails);
            _context.SaveChanges();
        }

        private void DeletePurchaseDetails(long headerId, EntitiesDbContext context)
        {
            IEnumerable<PurechasesDetials> purchaseDetails = context.PurechasesDetials.Where(x => x.PurechasesHeaderId == headerId);
            context.PurechasesDetials.RemoveRange(purchaseDetails);
            context.SaveChanges();
        }
        private decimal GetTotalPurechase()
        {
            return GetAllDaily().Sum(x => x.Total);
        }
        private int GetTodayTotalQuantity()
        {
            return _context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Quantity);
        }
        private decimal GetTodayTotalCommission()
        {
            return GetAllDaily().Sum(x => x.Commission);
        }
        private decimal GetTodayTotalNawlon()
        {
            return GetAllDaily().Sum(x => x.Nawlon);
        }
        private int GetTodayTotalWeight()
        {
            return _context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Weight);
        }
        private decimal GetTodayTotalPrice()
        {
            return _context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Price);
        }
        public bool UpdateInPrinting(PurechasesHeader entity)
        {
            PurechasesHeader purechaseHeader = GetById(entity.Id);
            purechaseHeader.Commission = entity.Commission;
            purechaseHeader.Nawlon = entity.Nawlon;
            purechaseHeader.CommissionRate = entity.CommissionRate;
            purechaseHeader.Gift = entity.Gift;
            purechaseHeader.Descent = entity.Descent;
            purechaseHeader.Expense = entity.Expense;
            purechaseHeader.IsPrinted = true;

            _context.Entry(purechaseHeader).State = EntityState.Modified;
            _context.SaveChanges();
            _safeOperationsRepo.UpdateByHeaderId(entity.Id, entity.Total, AccountTypesEnum.Clients);
            return true;
        }
        #endregion

    }
}
