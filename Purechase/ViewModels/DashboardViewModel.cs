using System;
using System.Collections.Generic;
using System.Text;

namespace Purechase.ViewModels
{
   public class DashboardViewModel
    {
        public decimal TotalPurchase { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalDescent { get; set; }
        public decimal TotalGift { get; set; }
    }
}
