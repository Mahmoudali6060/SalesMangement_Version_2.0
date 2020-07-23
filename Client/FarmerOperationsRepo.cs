using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Farmers.DTOs;
using Microsoft.EntityFrameworkCore;
using Safes;
using Shared.Enums;

namespace Farmers
{
    public class FarmerOperationsRepo : IFarmerOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<Farmer> _farmerEntity;
        private readonly ISafeOperationsRepo _safeOperationsRepo;
        public FarmerOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo)
        {
            _context = context;
            _farmerEntity = context.Set<Farmer>();
            _safeOperationsRepo = safeOperationsRepo;
        }


        public IEnumerable<Farmer> GetAll()
        {
            return _farmerEntity.Include("PurechasesHeader").AsEnumerable().OrderBy(x => x.Name);
        }
        public FarmListDTO GetAll(int currentPage, string keyword)
        {
            var list = _farmerEntity
                .Include("PurechasesHeader")
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            return new FarmListDTO()
            {
                Total = _farmerEntity.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize).OrderBy(x=>x.Name)
            };
        }
        public Farmer GetById(long id)
        {
            return _farmerEntity.SingleOrDefault(s => s.Id == id);
        }

        public long Add(Farmer farmer)
        {
            try
            {
                _context.Entry(farmer).State = EntityState.Added;
                _context.SaveChanges();
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
            _context.Entry(farmer).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            _safeOperationsRepo.DeleteByAccountId(id, AccountTypesEnum.Clients);
            Farmer farmer = GetById(id);
            _farmerEntity.Remove(farmer);
            _context.SaveChanges();
            return true;
        }


    }
}
