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
        bool Add(SalesinvoicesHeader entity, long orderHeaderId);
        bool Update(SalesinvoicesHeader entity, long orderHeaderId);
        bool Update(SalesinvoicesHeader entity);
        bool Delete(long id);
        void DeleteSalesinvoiceDetails(OrderHeader orderHeader);
        void DeleteSalesinvoiceHeader(OrderHeader orderHeader);

    }
}
