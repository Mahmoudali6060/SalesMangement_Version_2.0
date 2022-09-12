using Database;
using Database.Entities;
using Safes.DTOs;
using Shared.Enums;
using Shared.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Safes
{
    public interface ISafeOperationsRepo
    {
        IEnumerable<Safe> GetAll();
        SafeDTO GetAll(int currentPage, string keyword);
        Safe GetById(long id);
        long Add(Safe entity);
        long Add(Safe entity, EntitiesDbContext context);
        bool Update(Safe entity);
        bool Delete(long id);
        IEnumerable<Safe> GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum);
        SafeDTO GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, int currentPage);
        bool DeleteByHeaderId(long header, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);
        bool UpdateByHeaderId(long header, decimal total, AccountTypesEnum accountTypesEnum);
        bool UpdateByHeaderId(long header, decimal total, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);
        bool DeleteByOrderId(long orderId, EntitiesDbContext context);
        bool DeleteByAccountId(long accountId, AccountTypesEnum accountTypesEnum);
        BalanceDTO GetBalanceByAccountId(long accountId, AccountTypesEnum accountTypesEnum);
        bool TransferToSafe(long headerId, decimal total, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);

    }
}
