using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Safes.DTOs
{
    public class BalanceDTO
    {
        public decimal TotalIncoming { get; set; }
        public decimal TotalOutcoming { get; set; }
    }
}
