using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Users
{
   public interface IUserOperationsRepo
    {
        IEnumerable<User> GetAll();
        User GetById(long id);
        bool Add(User entity);
        bool Update(User entity);
        bool Delete(long id);
        User Login(string username,string password);
        bool Logout();
    }
}
