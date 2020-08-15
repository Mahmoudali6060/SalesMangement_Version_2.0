using Database;
using Database.Entities;
using Purechase.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purechase
{
    public interface IPurechasesOperationsRepo
    {
        IEnumerable<PurechasesHeader> GetAll();
        PurechaseListDTO GetAll(int currentPage, string keyword, bool isToday);
        PurechasesHeader GetById(long id);
        bool Add(PurechasesHeader entity, EntitiesDbContext context);
        bool Update(PurechasesHeader entity, EntitiesDbContext context);
        bool Update(PurechasesHeader entity);
        bool UpdateInPrinting(PurechasesHeader purechasesHeader);
        bool Delete(long id, EntitiesDbContext context);
        void DeleteRelatedPurechase(long orderHeaderId, EntitiesDbContext context);
        DashboardDTO GetDashboardData();
        IEnumerable<PurechasesHeader> GetPurchaseHeaderListByFarmerId(long farmerId);
    }
}
