using System;
using Database.Backup;
using Database.Entities;
using Safes;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class SafesController : Controller
    {
        private ISafeOperationsRepo _safeOperationsRepo;

        public SafesController(ISafeOperationsRepo safeOperationsRepo)
        {
            this._safeOperationsRepo = safeOperationsRepo;
        }

        public ActionResult Index()
        {
            //BackupService backupService = new BackupService("Server=.\\SQLEXPRESS;Database=TabarakDb;Trusted_Connection=True;", @"F:\Backup");
            //backupService.BackupDatabase("TabarakDb");
            return View();
        }

        public JsonResult List()
        {
            //try
            //{
                var safeList = Helper.SerializeObject(_safeOperationsRepo.GetAll());
                return Json(safeList);
            //}
            //catch (Exception ex)
            //{
            //    System.IO.File.WriteAllText(@"e:\errorLog.txt", ex.Message);
            //    throw ex;
            //}

        }

        public JsonResult GetById(long id)
        {
            var safe = Helper.SerializeObject(_safeOperationsRepo.GetById(id));
            return Json(safe);
        }

        public JsonResult Add(Safe safe)
        {
          
            return Json(_safeOperationsRepo.Add(safe));
        }

        public JsonResult Update([FromBody] Safe safe)
        {
            return Json(_safeOperationsRepo.Update(safe));
        }

        public JsonResult Delete(long id)
        {
            return Json(_safeOperationsRepo.Delete(id));
        }
    }
}