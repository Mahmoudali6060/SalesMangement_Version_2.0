using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Purechase;
using Safes;
using Farmers.DTOs;
using Shared.Enums;

namespace SalesManagementApp.Controllers
{
    public class FarmerAccountStatementController : Controller
    {
        private ISafeOperationsRepo _safeOperationsRepo;

        public FarmerAccountStatementController(ISafeOperationsRepo safeOperationsRepo)
        {
            _safeOperationsRepo = safeOperationsRepo;
        }

        public ActionResult Index(long farmerId)
        {
            ViewBag.farmerId = farmerId;
            return View();
        }

        public JsonResult List(long farmerId, string dateFrom, string dateTo)
        {
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(farmerId, AccountTypesEnum.Clients,dateFrom,dateTo));
            return Json(safeList);
        }

        public JsonResult GetPagedList(long farmerId, int currentPage, string dateFrom, string dateTo)
        {
            var safeDto = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(farmerId, AccountTypesEnum.Clients, currentPage, dateFrom, dateTo));
            return Json(safeDto);
        }

    }
}