using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Purechase.DTOs;
using Safes;
using Shared.Enums;

namespace Purechase
{
    public class PurechasesOperationsRepo : IPurechasesOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<PurechasesHeader> _purechasesHeaderEntity;
        private ISafeOperationsRepo _safeOperationsRepo;
        public PurechasesOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo)
        {
            this.context = context;
            _purechasesHeaderEntity = context.Set<PurechasesHeader>();
            _safeOperationsRepo = safeOperationsRepo;
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
                list = list.Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
                total = _purechasesHeaderEntity.Count(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString());
            }
            else
            {
                total = _purechasesHeaderEntity.Count();
            }

            return new PurechaseListDTO()
            {
                Total = total,
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }

        public IEnumerable<PurechasesHeader> GetAllDaily()
        {
            return _purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.Id);
        }

        public PurechasesHeader GetById(long id)
        {
            return _purechasesHeaderEntity.SingleOrDefault(s => s.Id == id);
        }

        public bool Add(PurechasesHeader purechasesHeader)
        {
            context.Entry(purechasesHeader).State = EntityState.Added;
            context.SaveChanges();
            SetPurechasesHeaderId(purechasesHeader.Id, purechasesHeader.PurechasesDetialsList);
            AddPurechasesDetials(purechasesHeader.PurechasesDetialsList);
            return true;
        }

        public bool Update(PurechasesHeader purechasesHeader)
        {
            context.Entry(purechasesHeader).State = EntityState.Modified;
            context.SaveChanges();
            DeletePurchaseDetails(purechasesHeader.Id);
            SetPurechasesHeaderId(purechasesHeader.Id, purechasesHeader.PurechasesDetialsList);
            AddPurechasesDetials(purechasesHeader.PurechasesDetialsList);
            _safeOperationsRepo.UpdateByHeaderId(purechasesHeader.Id, purechasesHeader.Total, AccountTypesEnum.Clients);
            return true;
        }

        public bool Delete(long id)
        {
            PurechasesHeader purechasesHeader = GetById(id);
            DeletePurchaseDetails(id);
            _purechasesHeaderEntity.Remove(purechasesHeader);
            context.SaveChanges();
            return true;
        }

        public void DeleteRelatedPurechase(long orderHeaderId)
        {
            Order_Purechase order_Purechase = context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
            if (order_Purechase != null && order_Purechase.PurechasesHeaderId != 0)
            {
                PurechasesHeader purechasesHeader = context.PurechasesHeaders.FirstOrDefault(x => x.Id == order_Purechase.PurechasesHeaderId);
                Delete(purechasesHeader.Id);
            }

        }

        public DashboardDTO GetDashboardData()
        {
            int totalQuantity = GetTodayTotalQuantity();
            decimal totalCommission = GetTodayTotalCommission();
            decimal nawlon = GetTodayTotalNawlon();

            return new DashboardDTO()
            {
                TotalPurchase = Math.Ceiling(GetTotalPurechase()),
                TotalCommission = Math.Ceiling(totalCommission),
                TotalGift = Math.Ceiling(0.5M * totalQuantity),
                TotalDescent = totalQuantity
            };
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

        private void AddPurechasesDetials(IEnumerable<PurechasesDetials> purechasesDetialsList)
        {
            context.PurechasesDetials.AddRange(purechasesDetialsList);
            context.SaveChanges();
        }

        private void DeletePurchaseDetails(long headerId)
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
            return context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Quantity);
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
            return context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Weight);
        }

        private decimal GetTodayTotalPrice()
        {
            return context.PurechasesDetials.AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).Sum(x => x.Price);
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

            context.Entry(purechaseHeader).State = EntityState.Modified;
            context.SaveChanges();
            _safeOperationsRepo.UpdateByHeaderId(entity.Id, entity.Total, AccountTypesEnum.Clients);
            return true;
        }

        #endregion

    }
}
