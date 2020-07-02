using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Farmers
{
    public class FarmerOperationsRepo : IFarmerOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<Farmer> farmerEntity;

        public FarmerOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            farmerEntity = context.Set<Farmer>();
        }


        public IEnumerable<Farmer> GetAll()
        {
            return farmerEntity.Include("PurechasesHeader").AsEnumerable();
        }

        public Farmer GetById(long id)
        {
            return farmerEntity.SingleOrDefault(s => s.Id == id);
        }

        public long Add(Farmer farmer)
        {
            try
            {
                context.Entry(farmer).State = EntityState.Added;
                context.SaveChanges();
                return farmer.Id;
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }

        }

        public bool Update(Farmer farmer)
        {
            context.Entry(farmer).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            Farmer farmer = GetById(id);
            farmerEntity.Remove(farmer);
            context.SaveChanges();
            return true;
        }
    }
}
