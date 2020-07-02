using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Farmer : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        //public decimal Balance { get; set; }
        public virtual ICollection<OrderHeader> OrderHeader { get; set; }
        public virtual ICollection<PurechasesHeader> PurechasesHeader { get; set; }

    }
}
