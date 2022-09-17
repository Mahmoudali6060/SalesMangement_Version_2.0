using System;
using Database.Backup;
using Database.Entities;
using Safes;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Shared.Enums;

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
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetAll());
            return Json(safeList);
        }

        public JsonResult GetById(long id)
        {
            var safe = Helper.SerializeObject(_safeOperationsRepo.GetById(id));
            return Json(safe);
        }

        public JsonResult GetPagedList(int currentPage, string dateFrom, string dateTo, AccountTypesEnum accountTypesId)
        {
            return Json(Helper.SerializeObject(_safeOperationsRepo.GetAll(currentPage, dateFrom,dateTo,accountTypesId)));
        }

        public JsonResult GetBalanceByAccountId(long accountId,AccountTypesEnum accountTypesEnum)
        {
            var safe = Helper.SerializeObject(_safeOperationsRepo.GetBalanceByAccountId(accountId,accountTypesEnum));
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