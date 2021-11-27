using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shared.Classes;
using Account.Users;
using Account.Account;
using Account.Models;
using Database.Backup;
using Newtonsoft.Json;

namespace SalesManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private IAccountRepo _accountRepo;

        public AccountController(IAccountRepo accountRepo)
        {
            this._accountRepo = accountRepo;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            User user = _accountRepo.Login(username, password);
            if (user == null)
                return View();
            else
            {
                LoggedUser loggedUser = _accountRepo.RedirectLoggedUser(user);

                HttpContext.Session.SetString("_LoggedUserRole", user.Role.Name);
                HttpContext.Session.SetString("_LoggedUserName", user.Username);
                HttpContext.Session.SetString("_ImagesUrl", user.Company.LogoUrl);
                HttpContext.Session.SetString("_UserFullName", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("_ProfileImageUrl", user.ImageUrl);

                //user.Role = null;
                //user.Company = null;
                //HttpContext.Session.SetString("_User", JsonConvert.SerializeObject(user));

                //// Retrieve
                //var str = HttpContext.Session.GetString(key);
                //var obj = JsonConvert.DeserializeObject<User>(str);

                return RedirectToAction(loggedUser.ActionName, loggedUser.ControllerName);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }
}