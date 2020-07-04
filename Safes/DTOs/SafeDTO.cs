using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Safes.DTOs
{
    public class SafeDTO
    {
        public IEnumerable<Safe> List { get; set; }
        public int Total { get; set; }
    }
}
