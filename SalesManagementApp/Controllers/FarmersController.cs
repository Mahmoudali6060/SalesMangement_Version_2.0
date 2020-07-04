using System;
using Database.Backup;
using Database.Entities;
using Farmers;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    public class FarmersController : Controller
    {
        private IFarmerOperationsRepo _farmerOperationsRepo;

        public FarmersController(IFarmerOperationsRepo farmerOperationsRepo)
        {
            this._farmerOperationsRepo = farmerOperationsRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(int currentPage)
       {
            var farmerList = Helper.SerializeObject(_farmerOperationsRepo.GetAll(currentPage));
            return Json(farmerList);
        }

        public JsonResult GetById(long id)
        {
            var farmer = Helper.SerializeObject(_farmerOperationsRepo.GetById(id));
            return Json(farmer);
        }

        public JsonResult Add(Farmer farmer)
        {
            return Json(_farmerOperationsRepo.Add(farmer));
        }

        public JsonResult Update([FromBody] Farmer farmer)
        {
            return Json(_farmerOperationsRepo.Update(farmer));
        }

        public JsonResult Delete(long id)
        {
            return Json(_farmerOperationsRepo.Delete(id));
        }
    }
}