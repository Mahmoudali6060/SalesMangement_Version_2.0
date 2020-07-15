using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sellers.DTOs
{
    public class SellerListDTO
    {
        public IEnumerable<Seller> List { get; set; }
        public int Total { get; set; }
    }
}
