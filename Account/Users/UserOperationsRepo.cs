using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Users
{
    public class UserOperationsRepo : IUserOperationsRepo
    {
        private EntitiesDbContext context;
        private DbSet<User> userEntity;

        public UserOperationsRepo(EntitiesDbContext context)
        {
            this.context = context;
            userEntity = context.Set<User>();
        }

        public IEnumerable<User> GetAll()
        {
            return userEntity.Include("Role").AsEnumerable();
        }

        public User GetById(long id)
        {
            return userEntity.Include("Role").SingleOrDefault(s => s.Id == id);
        }

        public bool Add(User user)
        {
            try
            {
                context.Entry(user).State = EntityState.Added;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }

        }

        public bool Update(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            User user = GetById(id);
            userEntity.Remove(user);
            context.SaveChanges();
            return true;
        }

        public User Login(string username, string password)
        {
            return context.Users.Include("Role").FirstOrDefault(x => x.Username == username && x.Password == username);
        }

        public bool Logout()
        {
            throw new NotImplementedException();
        }
    }
}
