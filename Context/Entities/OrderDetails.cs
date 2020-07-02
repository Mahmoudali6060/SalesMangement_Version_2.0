using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class OrderDetails : BaseEntity
    {
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public decimal SellingPrice { get; set; }
        public long SellerId { get; set; }
        public long OrderHeaderId { get; set; }

        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Seller Seller { get; set; }

    }
}
