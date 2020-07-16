using System;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using Salesinvoice;

namespace SalesManagementApp.Controllers
{
    public class SalesinvoicesController : Controller
    {
        private ISalesinvoicesOperationsRepo _salesinvoicesOperationsRepo;

        public SalesinvoicesController(ISalesinvoicesOperationsRepo salesinvoicesHeaderOperationsRepo)
        {
            this._salesinvoicesOperationsRepo = salesinvoicesHeaderOperationsRepo;
        }

        public ActionResult Index(int? today)
        {
            ViewBag.today = today;
            return View();
        }

        public JsonResult List()
        {
            var salesinvoicesHeaderList = Helper.SerializeObject(_salesinvoicesOperationsRepo.GetAll());
            return Json(salesinvoicesHeaderList);
        }
        public JsonResult GetPagedList(int currentPage, string keyword, bool isToday)
        {
            return Json(Helper.SerializeObject(_salesinvoicesOperationsRepo.GetAll(currentPage, keyword, isToday)));
        }

        public JsonResult GetById(long id)
        {
            var salesinvoicesHeader = Helper.SerializeObject(_salesinvoicesOperationsRepo.GetById(id));
            return Json(salesinvoicesHeader);
        }

        public JsonResult Add(SalesinvoicesHeader salesinvoicesHeader)
        {
            return Json(_salesinvoicesOperationsRepo.Add(salesinvoicesHeader,0));
        }

        public JsonResult Update(SalesinvoicesHeader salesinvoicesHeader)
        {
            return Json(_salesinvoicesOperationsRepo.Update(salesinvoicesHeader));
        }

        public JsonResult Delete(long id)
        {
            return Json(_salesinvoicesOperationsRepo.Delete(id));
        }
    }
}