﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
   public class Role : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
