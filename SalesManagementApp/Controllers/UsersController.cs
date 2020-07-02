using System;
using Database.Entities;
using Account.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class UsersController : Controller
    {
        private IUserOperationsRepo _userOperationsRepo;

        public UsersController(IUserOperationsRepo userOperationsRepo)
        {
            this._userOperationsRepo = userOperationsRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            try
            {
                var userList = Helper.SerializeObject(_userOperationsRepo.GetAll());
                return Json(userList);
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }

        }
        public JsonResult GetById(long id)
        {
            var user = Helper.SerializeObject(_userOperationsRepo.GetById(id));
            return Json(user);
        }

        public JsonResult Add(User user)
        {
            return Json(_userOperationsRepo.Add(user));
        }

        public JsonResult Update([FromBody] User user)
        {
            return Json(_userOperationsRepo.Update(user));
        }

        public JsonResult Delete(long id)
        {
            return Json(_userOperationsRepo.Delete(id));
        }
    }
}