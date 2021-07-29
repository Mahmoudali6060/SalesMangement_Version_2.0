using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Purechase;
using Shared.Classes;

namespace SalesManagementApp.Controllers
{
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

        public JsonResult List(string selectedDate)
        {
            var dashboard = Helper.SerializeObject(_purechasesOperationsRepo.GetDashboardData(DateTime.Parse(selectedDate)));
            return Json(dashboard);
        }

    }
}
