using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class OrderHeader : BaseEntity
    {
        public string Number { get; set; }
        public DateTime OrderDate { get; set; }
        public long FarmerId { get; set; }
        public virtual Farmer Farmer { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        //public virtual ICollection<SalesinvoicesHeader> SalesinvoicesHeadersList { get; set; }
        //public virtual ICollection<SalesinvoicesDetials> SalesinvoicesDetialsList { get; set; }

    }
}
