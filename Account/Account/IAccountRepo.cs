using Account.Models;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Account
{
   public interface IAccountRepo 
    {
        User Login(string username,string password);
        LoggedUser RedirectLoggedUser(User user);
        bool Logout();
    }
}
