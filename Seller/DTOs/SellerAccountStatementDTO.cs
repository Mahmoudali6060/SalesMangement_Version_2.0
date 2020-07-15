using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sellers.DTOs
{
    public class SellerAccountStatementDTO
    {
        public IEnumerable<Safe> SafeList { get; set; }
        public IEnumerable<SalesinvoicesHeader> SalesinvoicesHeaderList { get; set; }
    }
}
