using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; }
        public long? CompanyId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Company Company { get; set; }

    }
}
