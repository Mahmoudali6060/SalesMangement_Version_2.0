using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Seller : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public virtual ICollection<SalesinvoicesHeader> SalesinvoicesHeaders { get; set; }

    }
}
