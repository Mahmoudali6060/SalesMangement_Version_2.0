using System;
using System.Collections.Generic;
using System.Text;

namespace Purechase.DTOs
{
   public class DashboardDTO
    {
        public decimal TotalPurchase { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalDescent { get; set; }
        public decimal TotalGift { get; set; }
        public decimal TotalSalesinvoice { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalSalesWeight { get; set; }
        public int TotalPurchaseWeight { get; set; }
        public decimal TotalClientsAccountStatement { get; set; }
        public decimal TotalSellersAccountStatement { get; set; }
    }
}
