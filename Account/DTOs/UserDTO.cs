using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class UserDTO : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long RoleId { get; set; }
        public long? CompanyId { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageUrl { get; set; }

    }
}
