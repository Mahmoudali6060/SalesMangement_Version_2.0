using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
   public class PurechasesHeader : BaseEntity
    {
        public DateTime PurechasesDate { get; set; }
        public long  FarmerId { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal Commission { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal Nawlon { get; set; }
        public decimal Gift { get; set; }
        public decimal Descent { get; set; }
        public decimal? Expense { get; set; }
        public bool? IsPrinted { get; set; }
        public virtual ICollection<PurechasesDetials> PurechasesDetialsList { get; set; }
        public virtual Farmer Farmer{ get; set; }

    }
}
