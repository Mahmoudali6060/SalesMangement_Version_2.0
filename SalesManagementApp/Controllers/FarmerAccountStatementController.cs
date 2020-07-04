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
        private IPurechasesOperationsRepo _purechasesOperationsRepo;
        private ISafeOperationsRepo _safeOperationsRepo;

        public FarmerAccountStatementController(IPurechasesOperationsRepo purechasesHeaderOperationsRepo, ISafeOperationsRepo safeOperationsRepo)
        {
            this._purechasesOperationsRepo = purechasesHeaderOperationsRepo;
            _safeOperationsRepo = safeOperationsRepo;
        }

        public ActionResult Index(long farmerId)
        {
            ViewBag.farmerId = farmerId;
            return View();
        }

        public JsonResult List(long farmerId)
        {
            //FarmerAccountStatementDTO farmerAccountStatement = new FarmerAccountStatementDTO()
            //{
            var safeList = Helper.SerializeObject(_safeOperationsRepo.GetByAccountId(farmerId,AccountTypesEnum.Clients));
            //    PurechasesHeaderList = _purechasesOperationsRepo.GetPurchaseHeaderListByFarmerId(farmerId)
            //};
            return Json(safeList);
        }
    }
}