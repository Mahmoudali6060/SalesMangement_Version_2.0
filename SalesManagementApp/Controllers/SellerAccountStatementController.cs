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

        public JsonResult List(long sellerId, string dateFrom, string dateTo)
        {
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(sellerId, AccountTypesEnum.Sellers,dateFrom,dateTo));
            return Json(safeList);
        }

        public JsonResult GetPagedList(long sellerId, int currentPage, string dateFrom, string dateTo)
        {
            var safeDto = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(sellerId, AccountTypesEnum.Sellers, currentPage, dateFrom, dateTo));
            return Json(safeDto);
        }

    }
}