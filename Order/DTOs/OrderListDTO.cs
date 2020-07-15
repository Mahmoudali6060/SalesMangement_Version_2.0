using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.DTOs
{
    public class OrderListDTO
    {
        public IEnumerable<OrderHeader> List { get; set; }
        public int Total { get; set; }
    }
}
