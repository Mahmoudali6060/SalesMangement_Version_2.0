using Database.Entities;
using Purechase.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purechase
{
   public interface IPurechasesOperationsRepo 
    {
        IEnumerable<PurechasesHeader> GetAll();
        IEnumerable<PurechasesHeader> GetAllDaily();
        PurechasesHeader GetById(long id);
        bool Add(PurechasesHeader entity);
        bool Update(PurechasesHeader entity);
        bool UpdateInPrinting(PurechasesHeader purechasesHeader);
        bool Delete(long id);
        void DeleteRelatedPurechase(long orderHeaderId);
        DashboardViewModel GetDashboardData();
        IEnumerable<PurechasesHeader> GetPurchaseHeaderListByFarmerId(long farmerId);
    }
}
