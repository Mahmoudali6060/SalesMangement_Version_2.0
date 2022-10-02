using Database;
using Database.Entities;
using Farmers.DTOs;
using Shared.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmers
{
    public interface IFarmerOperationsRepo
    {
        IEnumerable<Farmer> GetAll();
        FarmListDTO GetAll(int currentPage, string keyword);
        Farmer GetById(long id);
        long Add(Farmer entity);
        bool Update(Farmer entity);
        bool Delete(long id);
        bool UpdateBalance(long farmerId, decimal balance, EntitiesDbContext context);

    }
}
