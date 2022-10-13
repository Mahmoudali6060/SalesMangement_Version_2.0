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
        SafeDTO GetAll(int currentPage, string dateFrom, string dateTo, AccountTypesEnum accountTypesId);
        Safe GetById(long id);
        long Add(Safe entity);
        bool SaveRange(List<Safe> safeList);

        long Add(Safe entity, EntitiesDbContext context);
        bool Update(Safe entity);
        bool Delete(long id);
        IEnumerable<Safe> GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, string dateFrom, string dateTo);
        SafeDTO GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, int currentPage, string dateFrom, string dateTo);
        bool DeleteByHeaderId(long header, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);
        bool UpdateByHeaderId(long header, decimal total, AccountTypesEnum accountTypesEnum);
        bool UpdateByHeaderId(long header, decimal total, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);
        bool DeleteByOrderId(long orderId, EntitiesDbContext context);
        bool DeleteByAccountId(long accountId, AccountTypesEnum accountTypesEnum);
        bool DeleteByAccountId(long accountId, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);

        BalanceDTO GetBalanceByAccountId(long accountId, AccountTypesEnum accountTypesEnum);
        bool TransferToSafe(PurechasesHeader purechasesHeader, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);
        bool UpdateBySalesinvoiceHeaderId(long salesinvoiceHeaderId, decimal total, EntitiesDbContext context);
        bool DeleteByOrder(OrderHeader orderHeader, AccountTypesEnum accountTypesEnum, EntitiesDbContext context);

    }
}
