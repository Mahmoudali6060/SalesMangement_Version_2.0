using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Safes
{
    public class SafeOperationsRepo : ISafeOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<Safe> safeEntity;

        public SafeOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            safeEntity = context.Set<Safe>();
        }


        public IEnumerable<Safe> GetAll()
        {
            return safeEntity.AsEnumerable();
        }

        public Safe GetById(long id)
        {
            return safeEntity.SingleOrDefault(s => s.Id == id);
        }

        public long Add(Safe safe)
        {
            try
            {
                context.Entry(safe).State = EntityState.Added;
                context.SaveChanges();
                return safe.Id;
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }

        }

        public bool Update(Safe safe)
        {
            context.Entry(safe).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            Safe safe = GetById(id);
            safeEntity.Remove(safe);
            context.SaveChanges();
            return true;
        }
    }
}
