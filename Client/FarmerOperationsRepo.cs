using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Farmers.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

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
        public FarmListDTO GetAll(int currentPage, string keyword)
        {
            var list = farmerEntity
                .Include("PurechasesHeader")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            return new FarmListDTO()
            {
                Total = farmerEntity.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
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
