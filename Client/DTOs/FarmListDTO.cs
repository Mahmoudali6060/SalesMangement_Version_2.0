using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmers.DTOs
{
    public class FarmListDTO
    {
        public IEnumerable<Farmer> List { get; set; }
        public int Total { get; set; }
    }
}
