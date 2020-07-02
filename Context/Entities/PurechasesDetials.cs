using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class PurechasesDetials :BaseEntity
    {
        public int Quantity { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }

        public long PurechasesHeaderId { get; set; }
        public virtual PurechasesHeader PurechasesHeader { get; set; }
    }
}
