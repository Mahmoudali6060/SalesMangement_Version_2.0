using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Roles
{
    public class RoleOperationsRepo : IRoleOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<Role> roleEntity;

        public RoleOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            roleEntity = context.Set<Role>();
        }

       
        public IEnumerable<Role> GetAll()
        {
            return roleEntity.AsEnumerable();
        }

        public Role GetById(long id)
        {
            return roleEntity.SingleOrDefault(s => s.Id == id);
        }

        public bool Add(Role role)
        {
            try
            {
                context.Entry(role).State = EntityState.Added;
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }
          
        }

        public bool Update(Role role)
        {
            context.Entry(role).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            Role role = GetById(id);
            roleEntity.Remove(role);
            context.SaveChanges();
            return true;
        }
    }
}
