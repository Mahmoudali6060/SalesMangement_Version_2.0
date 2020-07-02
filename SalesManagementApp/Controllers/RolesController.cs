using System;
using Database.Entities;
using Account.Roles;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class RolesController : Controller
    {
        private IRoleOperationsRepo _roleOperationsRepo;

        public RolesController(IRoleOperationsRepo roleOperationsRepo)
        {
            this._roleOperationsRepo = roleOperationsRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            try
            {
                var roleList = Helper.SerializeObject(_roleOperationsRepo.GetAll());
                return Json(roleList);
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
                throw ex;
            }

        }

        public JsonResult GetById(long id)
        {
            var role = Helper.SerializeObject(_roleOperationsRepo.GetById(id));
            return Json(role);
        }

        public JsonResult Add(Role role)
        {
            return Json(_roleOperationsRepo.Add(role));
        }

        public JsonResult Update([FromBody] Role role)
        {
            return Json(_roleOperationsRepo.Update(role));
        }

        public JsonResult Delete(long id)
        {
            return Json(_roleOperationsRepo.Delete(id));
        }
    }
}