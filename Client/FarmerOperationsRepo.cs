using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Farmers.DTOs;
using Microsoft.EntityFrameworkCore;
using Safes;
using Safes.DTOs;
using Salesinvoice;
using Shared.Enums;

namespace Farmers
{
    public class FarmerOperationsRepo : IFarmerOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<Farmer> _farmerEntity;
        private readonly ISafeOperationsRepo _safeOperationsRepo;
        private ISalesinvoicesOperationsRepo _salesinvoicesOperationsRepo;

        public FarmerOperationsRepo(EntitiesDbContext context, ISafeOperationsRepo safeOperationsRepo, ISalesinvoicesOperationsRepo salesinvoicesOperationsRepo)
        {
            _context = context;
            _farmerEntity = context.Set<Farmer>();
            _salesinvoicesOperationsRepo = salesinvoicesOperationsRepo;
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
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Name.Contains(keyword) || x.Address.Contains(keyword));
            }

            FarmListDTO farmListDTO = new FarmListDTO()
            {
                Total = list.Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };

            //foreach (var farmer in farmListDTO.List)
            //{
            //    BalanceDTO balanceDTO = _safeOperationsRepo.GetBalanceByAccountId(farmer.Id, AccountTypesEnum.Clients);
            //    farmer.Balance = balanceDTO.TotalIncoming - balanceDTO.TotalOutcoming;
            //}

            return farmListDTO;
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
                Safe safe = PrepareFarmerSafeEntity(farmer);
                _safeOperationsRepo.Add(safe);
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
            _safeOperationsRepo.DeleteByAccountId(id, AccountTypesEnum.Clients, _context);
            _salesinvoicesOperationsRepo.DeleteByFarmerId(id, _context);
            Farmer farmer = GetById(id);
            _farmerEntity.Remove(farmer);
            _context.SaveChanges();
            return true;
        }

        private Safe PrepareFarmerSafeEntity(Farmer farmer)
        {
            return new Safe()
            {
                Date = DateTime.Now.Date,
                AccountId = farmer.Id,
                AccountTypeId = (int)AccountTypesEnum.Clients,
                Outcoming = farmer.Balance,
                Notes = $"رصيد افتتاحي",
                IsHidden = false,
                IsTransfered = true,
                HeaderId = 0,
                OrderId = 0
            };
        }

        public bool UpdateBalance(long farmerId, decimal balance, EntitiesDbContext context)
        {
            var exsitedFarmer = GetById(farmerId);
            exsitedFarmer.Balance += balance;
            context.Entry(exsitedFarmer).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }
    }
}
