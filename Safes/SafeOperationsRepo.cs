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
        private EntitiesDbContext context;
        private DbSet<Safe> safeEntity;

        public SafeOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            safeEntity = context.Set<Safe>();
        }


        public IEnumerable<Safe> GetAll()
        {
            return safeEntity.Where(x => x.IsHidden == false).AsEnumerable();
        }

        public SafeDTO GetAll(int currentPage, string keyword)
        {
            var list = safeEntity
                .OrderByDescending(x => x.Id)
                .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(x => x.Date.ToString("dd/MM/yyyy").Contains(keyword));
            }

            return new SafeDTO()
            {
                Total = safeEntity.Where(x => x.IsHidden == false).Count(),
                List = list.Skip((currentPage - 1) * PageSettings.PageSize).Take(PageSettings.PageSize)
            };
        }

        public Safe GetById(long id)
        {
            return safeEntity.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Safe> GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum)
        {
            return safeEntity.Where(s => s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum).AsEnumerable();
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
            if (safe != null)
            {
                safeEntity.Remove(safe);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteByHeaderId(long header, AccountTypesEnum accountTypesEnum)
        {
            Safe safe = safeEntity.SingleOrDefault(x => x.HeaderId == header && x.AccountTypeId == (int)accountTypesEnum);
            if (safe != null)
            {
                safeEntity.Remove(safe);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteByOrderId(long orderId)
        {
            List<Safe> safes = safeEntity.Where(x => x.OrderId == orderId).ToList();
            if (safes != null)
            {
                safeEntity.RemoveRange(safes);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public SafeDTO GetByAccountId(long accountId, AccountTypesEnum accountTypesEnum, int currentPage)
        {
            return new SafeDTO()
            {
                Total = safeEntity.Where(s => s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum).Count(),
                List = safeEntity
                 .Where(s => s.AccountId == accountId && s.AccountTypeId == (int)accountTypesEnum)
                .Skip((currentPage - 1) * PageSettings.PageSize)
                .Take(PageSettings.PageSize)
               .AsEnumerable()
            };
        }

        public bool DeleteByAccountId(long accountId, AccountTypesEnum accountTypesEnum)
        {
            List<Safe> safes = safeEntity.Where(x => x.AccountId == accountId && x.AccountTypeId == (int)accountTypesEnum).ToList();
            if (safes != null)
            {
                safeEntity.RemoveRange(safes);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
