using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Database.Backup;
using Microsoft.AspNetCore.Mvc;
using Purechase;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class DatabaseController : Controller
    {


        public DatabaseController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult BackupDatabase(DatabaseEntity databaseEntity)
        {
            BackupService backupService = new BackupService(databaseEntity.ConnectionString, databaseEntity.FilePath);
            return Json(backupService.BackupDatabase(databaseEntity.DatabaseName));
        }

        public JsonResult RestoreDatabase(DatabaseEntity databaseEntity)
        {
            BackupService backupService = new BackupService(databaseEntity.ConnectionString, databaseEntity.FilePath);
            return Json(backupService.RestoreDatabase(databaseEntity.DatabaseName));
        }


        public JsonResult FixSalesinvoiceTotal(DatabaseEntity databaseEntity)
        {
            BackupService backupService = new BackupService(databaseEntity.ConnectionString, databaseEntity.FilePath);
            return Json(backupService.FixSalesinvoiceTotal(databaseEntity.DatabaseName));
        }


        public JsonResult UpdateFarmersBalance(DatabaseEntity databaseEntity)
        {
            BackupService backupService = new BackupService(databaseEntity.ConnectionString, databaseEntity.FilePath);
            return Json(backupService.UpdateFarmersBalance());
        }

        public JsonResult UpdateSellersBalance(DatabaseEntity databaseEntity)
        {
            BackupService backupService = new BackupService(databaseEntity.ConnectionString, databaseEntity.FilePath);
            return Json(backupService.UpdateSellersBalance());
        }

    }
}
