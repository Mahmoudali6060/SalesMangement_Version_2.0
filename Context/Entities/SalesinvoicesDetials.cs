﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class SalesinvoicesDetials : BaseEntity
    {
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public long SalesinvoicesHeaderId { get; set; }
        public virtual SalesinvoicesHeader PurechasesHeader { get; set; }
    }
}