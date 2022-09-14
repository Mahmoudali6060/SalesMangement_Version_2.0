using Database;
using Database.Entities;
using Salesinvoice.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salesinvoice
{
    public interface ISalesinvoicesOperationsRepo
    {
        IEnumerable<SalesinvoicesHeader> GetAll();
        SalesinvoiceListDTO GetAll(int currentPage, string keyword, bool isToday);
        SalesinvoicesHeader GetById(long id);
        SalesinvoicesHeader GetById(long id, EntitiesDbContext context);
        SalesinvoicesHeader Add(SalesinvoicesHeader entity, long orderHeaderId, EntitiesDbContext context);
        bool Update(SalesinvoicesHeader entity, EntitiesDbContext context);
        bool Update(SalesinvoicesHeader entity);
        bool UpdateInPrinting(SalesinvoicesHeader salesinvoicesHeader);
        bool Delete(long id, EntitiesDbContext context);
        List<SalesinvoicesDetials> DeleteSalesinvoiceDetails(OrderHeader orderHeader, EntitiesDbContext context);
        void DeleteSalesinvoiceHeader(DateTime orderHeaderCreatedDate, EntitiesDbContext context);
        IEnumerable<SalesinvoicesHeader> GetAllDaily(DateTime? dateFrom=null, DateTime? dateTo = null);
    }
}
