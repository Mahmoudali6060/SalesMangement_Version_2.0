using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
   public class SalesinvoicesHeader : BaseEntity
    {
        public DateTime SalesinvoicesDate { get; set; }
        public long  SellerId { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal ByaaTotal { get; set; }
        public decimal MashalTotal { get; set; }
        public bool? IsPrinted { get; set; }

        public virtual ICollection<SalesinvoicesDetials> SalesinvoicesDetialsList { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
