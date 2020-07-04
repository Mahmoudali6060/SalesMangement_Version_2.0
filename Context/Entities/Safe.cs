using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Safe : BaseEntity
    {
        public DateTime Date { get; set; }
        public int AccountTypeId { get; set; }
        public long AccountId { get; set; }
        public string OtherAccountName { get; set; }
        public decimal Incoming { get; set; }
        public decimal Outcoming { get; set; }
        public string Notes { get; set; }
        public bool IsHidden { get; set; }
        public long HeaderId { get; set; }
        public long OrderId { get; set; }
    }
}
