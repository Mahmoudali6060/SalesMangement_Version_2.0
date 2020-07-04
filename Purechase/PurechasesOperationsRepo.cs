using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Purechase.ViewModels;

namespace Purechase
{
    public class PurechasesOperationsRepo : IPurechasesOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<PurechasesHeader> purechasesHeaderEntity;

        public PurechasesOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            purechasesHeaderEntity = context.Set<PurechasesHeader>();
        }

        public IEnumerable<PurechasesHeader> GetAll()
        {
            return purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().OrderByDescending(x => x.Id);
        }

        public IEnumerable<PurechasesHeader> GetAllDaily()
        {
            return purechasesHeaderEntity.Include("PurechasesDetialsList").AsEnumerable().Where(x => x.Created.ToShortDateString() == DateTime.Now.ToShortDateString()).OrderByDescending(x => x.Id);
        }

        public PurechasesHeader GetById(long id)
        {
            return purechasesHeaderEntity.SingleOrDefault(s => s.Id == id);
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
            return true;
        }

        public bool Delete(long id)
        {
            PurechasesHeader purechasesHeader = GetById(id);
            DeletePurchaseDetails(id);
            purechasesHeaderEntity.Remove(purechasesHeader);
            context.SaveChanges();
            return true;
        }

        public void DeleteRelatedPurechase(long orderHeaderId)
        {
            Order_Purechase order_Purechase = context.Order_Purechases.FirstOrDefault(x => x.OrderHeaderId == orderHeaderId);
            PurechasesHeader purechasesHeader = context.PurechasesHeaders.FirstOrDefault(x => x.Id == order_Purechase.PurechasesHeaderId);
            Delete(purechasesHeader.Id);
        }

        public DashboardViewModel GetDashboardData()
        {
            int totalQuantity = GetTodayTotalQuantity();
            decimal totalCommission = GetTodayTotalCommission();
            decimal nawlon = GetTodayTotalNawlon();

            return new DashboardViewModel()
            {
                TotalPurchase = Math.Ceiling(GetTotalPurechase()),
                TotalCommission = Math.Ceiling(totalCommission),
                TotalGift = Math.Ceiling(0.5M * totalQuantity),
                TotalDescent = totalQuantity
            };
        }

        public IEnumerable<PurechasesHeader> GetPurchaseHeaderListByFarmerId(long farmerId)
        {
            return purechasesHeaderEntity.Where(x => x.FarmerId == farmerId).Include("PurechasesDetialsList").AsEnumerable().OrderByDescending(x => x.PurechasesDate);
        }
        #region Helper
        private IEnumerable<PurechasesDetials> SetPurechasesHeaderId(long purechasesHeaderId, IEnumerable<PurechasesDetials> purechasesDetails)
        {
            foreach (var item in purechasesDetails)
            {
                item.PurechasesHeaderId = purechasesHeaderId;
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
            //purechaseHeader.Total += entity.Commission + entity.Nawlon;
            context.Entry(purechaseHeader).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        #endregion

    }
}
