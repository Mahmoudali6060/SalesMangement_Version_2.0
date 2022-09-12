using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Safes.DTOs;
using Shared.Enums;

namespace Safes
{
    public class SafeOperationsRepo : ISafeOperationsRepo
    {
        private EntitiesDbContext _context;
        private DbSet<Safe> _safeEntity;

        public SafeOperationsRepo(EntitiesDbContext context)
        {
            this._context = context;
            _safeEntity = context.Set<Safe>();
        }


        public IEnumerable<Safe> GetAll()
        {
            return _safeEntity.Where(x => x.IsHidden == false).AsEnumerable();
        }

        public SafeDTO GetAll(int currentPage, string keyword)
        {
            var list = _safeEntity
                .Where(x => x.IsHidden == false)
                .OrderByDescending(x => x.Id)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Date.ToString("dd/MM/yyyy").Contains(keyword));
            }

            return new SafeDTO()
            {
                Total = list.Where(x => x.IsHidden == false).Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }

        public Safe GetById(long id)
        {
            return _safeEntity.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Safe> GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, string dateFrom, string dateTo)
        {
            if (string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateTo))
            {
                return _safeEntity.Where(s => s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum).AsEnumerable();
            }
            return _safeEntity.Where(s => s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum && s.Date >= DateTime.Parse(dateFrom) && s.Date <= DateTime.Parse(dateTo)).AsEnumerable();
        }

        public long Add(Safe safe)
        {
            _context.Entry(safe).State = EntityState.Added;
            _context.SaveChanges();
            return safe.Id;
        }
        public long Add(Safe safe, EntitiesDbContext context)
        {
            context.Entry(safe).State = EntityState.Added;
            context.SaveChanges();
            return safe.Id;
        }

        public bool Update(Safe safe)
        {
            _context.Entry(safe).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            Safe safe = GetById(id);
            if (safe != null)
            {
                _safeEntity.Remove(safe);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteByHeaderId(long header, AccountTypesEnum accountTypesEnum, EntitiesDbContext context)
        {
            Safe safe = context.Safes.SingleOrDefault(x => x.HeaderId == header && x.AccountTypeId == (int)accountTypesEnum);
            if (safe != null)
            {
                context.Safes.Remove(safe);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateByHeaderId(long headerId, decimal total, AccountTypesEnum accountTypesEnum)
        {
            Safe safe = _safeEntity.SingleOrDefault(x => x.HeaderId == headerId && x.AccountTypeId == (int)accountTypesEnum);
            if (safe != null)
            {
                if (accountTypesEnum == AccountTypesEnum.Sellers) safe.Outcoming = total;
                else safe.Incoming = total;

                _safeEntity.Update(safe);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateByHeaderId(long headerId, decimal total, AccountTypesEnum accountTypesEnum, EntitiesDbContext context)
        {
            Safe safe = context.Safes.FirstOrDefault(x => x.HeaderId == headerId && x.AccountTypeId == (int)accountTypesEnum);
            if (safe != null)
            {
                if (accountTypesEnum == AccountTypesEnum.Sellers) safe.Outcoming = total;
                else safe.Incoming = total;

                context.Safes.Update(safe);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool TransferToSafe(long headerId, decimal total, AccountTypesEnum accountTypesEnum, EntitiesDbContext context)
        {
            Safe safe = context.Safes.FirstOrDefault(x => x.HeaderId == headerId && x.AccountTypeId == (int)accountTypesEnum);
            if (safe != null)
            {
                if (accountTypesEnum == AccountTypesEnum.Sellers) safe.Outcoming = total;
                else safe.Incoming = total;

                safe.IsTransfered = true;
                context.Safes.Update(safe);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteByOrderId(long orderId, EntitiesDbContext context)
        {
            List<Safe> safes = context.Safes.Where(x => x.OrderId == orderId).ToList();
            if (safes != null)
            {
                context.Safes.RemoveRange(safes);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public SafeDTO GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, int currentPage, string dateFrom, string dateTo)
        {
            IEnumerable<Safe> safes = null;
            if (string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateTo))
            {
                safes = _safeEntity
                        .Where(s => s.IsTransfered == true && s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum)
                        .OrderBy(x => x.Date);
            }
            else
            {
                safes = _safeEntity
                      .Where(s => s.IsTransfered == true && s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum && s.Date >= DateTime.Parse(dateFrom) && s.Date <= DateTime.Parse(dateTo))
                      .OrderBy(x => x.Date);
            }

            return new SafeDTO()
            {
                Total = _safeEntity.Where(s => s.IsTransfered == true && s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum).Count(),
                List = safes
                .Skip((currentPage - 1) * PageSettings.PageSize)
                .Take(PageSettings.PageSize)
               .AsEnumerable(),
                TotalIncoming = safes.Sum(x => Math.Ceiling(x.Incoming)),
                TotalOutcoming = safes.Sum(x => Math.Ceiling(x.Outcoming))
            };
        }

        public bool DeleteByAccountId(long accountId, AccountTypesEnum accountTypesEnum)
        {
            List<Safe> safes = _safeEntity.Where(x => x.AccountId == accountId && x.AccountTypeId == (int)accountTypesEnum).ToList();
            if (safes != null)
            {
                _safeEntity.RemoveRange(safes);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public BalanceDTO GetBalanceByAccountId(long accountId, AccountTypesEnum accountTypesEnum)
        {
            var safes = GetByAccountId(accountId, accountTypesEnum, null, null);
            decimal totalOutcoming = 0;
            decimal totalIncoming = 0;
            foreach (Safe safe in safes)
            {
                totalOutcoming += safe.Outcoming;
                totalIncoming += safe.Incoming;
            }
            return new BalanceDTO()
            {
                TotalIncoming = totalIncoming,
                TotalOutcoming = totalOutcoming
            };
        }
    }
}
