using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Models
{
    public class OrderDTO
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
