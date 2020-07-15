using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purechase.DTOs
{
    public class PurechaseListDTO
    {
        public IEnumerable<PurechasesHeader> List { get; set; }
        public int Total { get; set; }
    }
}
