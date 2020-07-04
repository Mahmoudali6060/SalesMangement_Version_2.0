using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Purechase;
using Safes;
using Shared.Enums;

namespace SalesManagementApp.Controllers
{
    public class SellerAccountStatementController : Controller
    {
        private ISafeOperationsRepo _safeOperationsRepo;

        public SellerAccountStatementController(ISafeOperationsRepo safeOperationsRepo)
        {
            _safeOperationsRepo = safeOperationsRepo;
        }

        public ActionResult Index(long sellerId)
        {
            ViewBag.sellerId = sellerId;
            return View();
        }

        public JsonResult List(long sellerId)
        {
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(sellerId, AccountTypesEnum.Clients));
            return Json(safeList);
        }

        public JsonResult List(long sellerId, int currentPage)
        {
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(sellerId, AccountTypesEnum.Clients, currentPage));
            return Json(safeList);
        }

    }
}