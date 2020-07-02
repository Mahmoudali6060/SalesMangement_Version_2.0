using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shared.Classes;
using Account.Users;
using Account.Account;
using Account.Models;
using Database.Backup;

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

                return RedirectToAction(loggedUser.ActionName, loggedUser.ControllerName);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Account");
        }

    }
}