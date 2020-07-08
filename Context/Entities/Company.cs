using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public virtual IEnumerable<User> Users { get; set; }
    }
}
