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
    public class HomeController : Controller
    {

        private IPurechasesOperationsRepo _purechasesOperationsRepo;
        //Testtllllllllkk
        public HomeController(IPurechasesOperationsRepo purechasesHeaderOperationsRepo)
        {
            this._purechasesOperationsRepo = purechasesHeaderOperationsRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public JsonResult List()
        {
            var dashboard = Helper.SerializeObject(_purechasesOperationsRepo.GetDashboardData());
            return Json(dashboard);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }
    }
}
