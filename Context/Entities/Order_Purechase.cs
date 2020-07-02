using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Entities
{
    public class Order_Purechase :BaseEntity
    {
        public long PurechasesHeaderId { get; set; }

        public long OrderHeaderId { get; set; }
        public virtual PurechasesHeader PurechasesHeaders { get; set; }

        public virtual OrderHeader OrderHeader { get; set; }
    }
}
