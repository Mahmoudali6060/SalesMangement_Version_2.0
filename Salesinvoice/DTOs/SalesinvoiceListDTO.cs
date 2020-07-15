using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Salesinvoice.DTOs
{
    public class SalesinvoiceListDTO
    {
        public IEnumerable<SalesinvoicesHeader> List { get; set; }
        public int Total { get; set; }
    }
}
