using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Purechase;
using Purechase.DTOs;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
    [Route("Transaction")]
    public class TransactionController : Controller
    {

        private IPurechasesOperationsRepo _purechasesOperationsRepo;
        public TransactionController(IPurechasesOperationsRepo purechasesHeaderOperationsRepo)
        {
            this._purechasesOperationsRepo = purechasesHeaderOperationsRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("GetDashboardData")]
        public JsonResult GetDashboardData(DateSearchDTO dateSearchDTO)
        {
            var dashboard = Helper.SerializeObject(_purechasesOperationsRepo.GetDashboardData(DateTime.Parse(dateSearchDTO.DateFrom), DateTime.Parse(dateSearchDTO.DateTo)));
            return Json(dashboard);
        }

        [HttpGet]
        [Route("TestAPI")]
        public JsonResult TestAPI()
        {
            return Json("Test");
        }

    }
}
