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

        public JsonResult List(long farmerId)
        {
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(farmerId, AccountTypesEnum.Clients));
            return Json(safeList);
        }

        public JsonResult GetPagedList(long farmerId, int currentPage)
        {
            var safeDto = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(farmerId, AccountTypesEnum.Clients, currentPage));
            return Json(safeDto);
        }

    }
}