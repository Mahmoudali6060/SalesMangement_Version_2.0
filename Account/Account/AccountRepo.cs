using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Account.Models;
using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Account
{
    public class AccountRepo : IAccountRepo
    {
        private EntitiesDbContext context;
        private DbSet<User> userEntity;

        public AccountRepo(EntitiesDbContext context)
        {
            this.context = context;
            userEntity = context.Set<User>();
        }
        public User Login(string username, string password)
        {
            return context.Users.Include("Role").FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public LoggedUser RedirectLoggedUser(User user)
        {
            switch (user.Role.Name)
            {
                case "مدير":
                    return new LoggedUser()
                    {
                        ControllerName = "Home",
                        ActionName = "Dashboard"
                    };
                case "موظف":
                    return new LoggedUser()
                    {
                        ControllerName = "Orders",
                        ActionName = "Index"
                    };
                default:
                    return new LoggedUser()
                    {
                        ControllerName = "Orders",
                        ActionName = "Index"
                    };
            }
        }


        public bool Logout()
        {
            throw new NotImplementedException();
        }


    }
}
