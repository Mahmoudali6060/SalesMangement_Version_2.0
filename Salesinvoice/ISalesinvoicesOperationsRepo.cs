using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salesinvoice
{
   public interface ISalesinvoicesOperationsRepo 
    {
        IEnumerable<SalesinvoicesHeader> GetAll();
        IEnumerable<SalesinvoicesHeader> GetAllDaily();
        SalesinvoicesHeader GetById(long id);
        bool Add(SalesinvoicesHeader entity);
        bool Update(SalesinvoicesHeader entity);
        bool Delete(long id);
        void DeleteSalesinvoiceDetails(OrderHeader orderHeader);
        void DeleteSalesinvoiceHeader(OrderHeader orderHeader);

    }
}
