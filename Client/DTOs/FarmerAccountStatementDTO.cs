using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmers.DTOs
{
    public class FarmerAccountStatementDTO
    {
        public IEnumerable<Safe> SafeList { get; set; }
        public IEnumerable<PurechasesHeader> PurechasesHeaderList { get; set; }
    }
}
